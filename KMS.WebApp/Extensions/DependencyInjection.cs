using Microsoft.AspNetCore.Identity;
using KMS.Core.Entities.Identity;
using KMS.Core.ViewModels.Content;
using KMS.Core.ViewModels.Identity;
using KMS.Data.Repositories.Content;
using KMS.Data.Repositories.Log;
using KMS.Data.SeedWorks;
using KMS.Data.Services;

namespace KMS.WebApp.Extensions
{
    public static class DependencyInjection
    {
        public static void AddDependencyInjection(this IServiceCollection services)
        {
            // Add services to the container.
            services.AddScoped(typeof(IRepositoryBase<>), typeof(RepositoryBase<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<SignInManager<AppUser>, SignInManager<AppUser>>();
            services.AddScoped<UserManager<AppUser>, UserManager<AppUser>>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<RoleManager<AppRole>, RoleManager<AppRole>>();

            services.AddScoped<ILogHistoryRepository, LogHistoryRepository>();
            services.AddScoped<ICompanyRepository, CompanyRepository>();
            services.AddScoped<IApplicationRepository, ApplicationRepository>();
            services.AddScoped<IKeyRepository, KeyRepository>();
            services.AddAutoMapper(typeof(AppUserViewModel));
        }
    }
}