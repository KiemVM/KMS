using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http.Extensions;
using Newtonsoft.Json;
using KMS.Common.Constants;

namespace KMS.WebApp.Extensions
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>() ?? new ExceptionHandlerFeature();
                    using HttpClient client = new HttpClient();
                    if (contextFeature.Error.Message.StartsWith("BadRequestException|"))
                    {
                        var message = contextFeature.Error.Message.Replace("BadRequestException|", "");
                        context.Response.StatusCode = StatusCodes.Status400BadRequest;
                        if (contextFeature.Path.ToLower().StartsWith("/api/"))
                        {
                            context.Response.ContentType = "application/json";
                            await context.Response.WriteAsync(message);
                        }
                        else
                        {
                            context.Response.Redirect("/Error?Message=" + message);
                        }
                    }
                    else
                    {
                        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                        HttpResponseMessage response = await client.PostAsJsonAsync(ConstUrl.ApiErrorCrm, new
                        {
                            Project = ConstSystem.ProjectName,
                            Content = JsonConvert.SerializeObject(contextFeature.Error),
                            Request = JsonConvert.SerializeObject(new
                            {
                                UserName = context.User.FindFirst(UserClaims.UserName)?.Value ?? "",
                                Url = context.Request.GetDisplayUrl(),
                                Message = contextFeature.Error.Message
                            })
                        });
                        string logCode = await response.Content.ReadAsStringAsync();
                        if (contextFeature.Path.ToLower().StartsWith("/api/"))
                        {
                            context.Response.ContentType = "application/json";
                            await context.Response.WriteAsync($"Mã lỗi: <b style=\"color: red;\">{logCode}</b> <p style=\"padding-top: 8px;\"> Vui lòng gửi mã lỗi cho IT hỗ trợ xử lý. </br> Chi tiết: <{contextFeature.Error.Message}> </p>");
                        }
                        else
                        {
                            context.Response.Redirect("/Error?logErrorId=" + logCode);
                        }
                    }
                });
            });
        }
    }
}