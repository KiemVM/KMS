using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.IdentityModel.Tokens;
using KMS.Core.Entities.Identity;
using KMS.Core.ViewModels.ConfigOptions;
using KMS.WebApp.Authorization;
using KMS.WebApp.Extensions;
using KMS.Data;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Net;
using System.Reflection;
using System.Text;
using KMS.Data.SeedWorks;
using KMS.Data.Repositories.Content;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("AppSettings.json", optional: true, reloadOnChange: true)
    .AddJsonFile($"AppSettings.{builder.Environment.EnvironmentName}.json", optional: true);
var configuration = builder.Configuration;

builder.Services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
builder.Services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();

var projectCorsPolicy = "ProjectCorsPolicy";
builder.Services.AddCors(o => o.AddPolicy(projectCorsPolicy, corsPolicyBuilder =>
{
    corsPolicyBuilder.AllowAnyMethod()
        .AllowAnyHeader()
        .WithOrigins(configuration["AllowedOrigins"]?.Split(";") ?? Array.Empty<string>())
        .AllowCredentials();
}));

var connectionString = configuration.GetConnectionString("DefaultConnection");

//Config DB Context and ASP.NET Core Identity
builder.Services.AddDbContext<KMSContext>(options => options.UseSqlServer(connectionString));

// **FIX 1: XÓA DUPLICATE AddDistributedSqlServerCache và chỉ giữ lại 1**
builder.Services.AddDistributedSqlServerCache(options =>
{
    options.ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.SchemaName = "dbo";
    options.TableName = "DatabaseCache";
    options.DefaultSlidingExpiration = TimeSpan.FromMinutes(30); // Tăng thời gian cache
});

// **FIX 2: THÊM DistributedMemoryCache cho Session (quan trọng!)**
builder.Services.AddDistributedMemoryCache();

builder.Services.AddIdentity<AppUser, AppRole>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<KMSContext>();

builder.Services.Configure<IdentityOptions>(options =>
{
    // Password settings.
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;

    // Lockout settings.
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = false;

    // User settings.
    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = true;
});

builder.Services.AddDependencyInjection();

//Auto mapper
//builder.Services.AddAutoMapper(typeof(FormDemoViewModel));

//Authen and author
builder.Services.Configure<JwtTokenSettings>(configuration.GetSection("JwtTokenSettings"));

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
    options.JsonSerializerOptions.WriteIndented = true;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.CustomOperationIds(apiDesc => apiDesc.TryGetMethodInfo(out MethodInfo methodInfo) ? methodInfo.Name : null);
    c.SwaggerDoc("WebApi", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Version = "v1",
        Title = "WebApi",
        Description = "WebApi"
    });
    c.ParameterFilter<SwaggerNullableParameterFilter>();
});

builder.Services.AddAuthentication(o =>
{
    o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(cfg =>
{
    cfg.RequireHttpsMetadata = false;
    cfg.SaveToken = true;
    cfg.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateLifetime = true,
        ClockSkew = TimeSpan.FromSeconds(0),
        ValidIssuer = configuration["JwtTokenSettings:Issuer"],
        ValidAudience = configuration["JwtTokenSettings:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtTokenSettings:Key"] ?? string.Empty))
    };
    cfg.Events = new JwtBearerEvents
    {
        OnMessageReceived = async context =>
        {
            var cache = context.HttpContext.RequestServices.GetRequiredService<IDistributedCache>();
            var token = context.Request.Cookies["Authorization"];

            if (!string.IsNullOrEmpty(token))
            {
                var cachedToken = await cache.GetStringAsync(token);
                if (string.IsNullOrEmpty(cachedToken))
                {
                    context.Fail("Invalid token");
                }
                else
                {
                    context.Token = cachedToken;
                }
            }
        }
    };
});

//Lúc trả ra API viết hoa chữ cái đầu của đối tượng
builder.Services.AddControllersWithViews().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
    options.JsonSerializerOptions.WriteIndented = true;
});

builder.Services.AddRazorPages();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/login";
    options.AccessDeniedPath = "/login";
    options.LogoutPath = "/logout";
    options.ExpireTimeSpan = TimeSpan.FromDays(1);
});

// **FIX 3: XÓA DUPLICATE Session config và chỉ giữ lại 1 cấu hình đầy đủ**
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(2); // Thời gian session expire
    options.Cookie.Name = "KMSPC.Session";
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true; // **QUAN TRỌNG**: Cho phép hoạt động mà không cần consent
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
    options.Cookie.SameSite = SameSiteMode.Lax;
    options.Cookie.Path = "/";
    options.Cookie.Domain = null; // null cho localhost
});


var app = builder.Build();

// Thêm session debug middleware inline
if (app.Environment.IsDevelopment())
{
    app.Use(async (context, next) =>
    {
        var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();

        // Log request
        logger.LogInformation($"=== REQUEST: {context.Request.Method} {context.Request.Path} ===");

        // Log cookies
        var cookies = context.Request.Cookies;
        logger.LogInformation($"COOKIES: {string.Join(", ", cookies.Select(c => $"{c.Key}={c.Value?[..Math.Min(c.Value.Length, 10)]}"))}");

        // Auto-init session for cart pages
        if (context.Request.Path.StartsWithSegments("/api/Cart") ||
            context.Request.Path.StartsWithSegments("/san-pham-pc") ||
            context.Request.Path.StartsWithSegments("/gio-hang"))
        {
            try
            {
                var sessionId = context.Session.Id;
                if (string.IsNullOrEmpty(context.Session.GetString("_Init")))
                {
                    context.Session.SetString("_Init", DateTime.Now.ToString());
                    logger.LogInformation($"SESSION CREATED: {sessionId}");
                }
                else
                {
                    logger.LogInformation($"SESSION EXISTS: {sessionId}");
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"SESSION ERROR: {ex.Message}");
            }
        }

        await next();

        // Log response
        logger.LogInformation($"RESPONSE: {context.Response.StatusCode}");
        var setCookies = context.Response.Headers.SetCookie;
        if (setCookies.Any())
        {
            logger.LogInformation($"SET-COOKIES: {string.Join(", ", setCookies)}");
        }
        logger.LogInformation("=== END REQUEST ===");
    });
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("WebApi/swagger.json", "Web Api");
        c.DisplayOperationId();
        c.DisplayRequestDuration();
    });
}

app.ConfigureExceptionHandler();
app.UseCors(projectCorsPolicy);
app.UseHttpsRedirection();
app.UseStaticFiles();


app.UseRouting();

// **FIX 4: UseSession phải đặt đúng vị trí**
app.UseSession(); // Sau UseRouting, trước UseAuthentication

app.UseAuthentication();

app.UseStatusCodePages(context =>
{
    var response = context.HttpContext.Response;
    if (response.StatusCode == (int)HttpStatusCode.Unauthorized || response.StatusCode == (int)HttpStatusCode.Forbidden)
        response.Redirect("/login");
    return Task.CompletedTask;
});

app.UseAuthorization();
app.MapRazorPages();
app.MapControllers();

app.MigrateDatabase();
app.Run();