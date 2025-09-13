using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Distributed;
using KMS.Common.Constants;
using Newtonsoft.Json;

namespace KMS.WebApp.Authorization
{
    public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    {
        private readonly IDistributedCache _distributedCache;

        public PermissionAuthorizationHandler(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            if (!context.User.Identity.IsAuthenticated)
                return;
            var roles = await _distributedCache.GetStringAsync($"Role_{context.User.Claims.FirstOrDefault(x => x.Type == UserClaims.Id)?.Value}") ?? "";
            if (roles.Contains(ConstSystem.RoleAdmin))
            {
                context.Succeed(requirement);
                return;
            }
            var permissions = JsonConvert.DeserializeObject<List<string>>(await _distributedCache.GetStringAsync($"Pms_{context.User.Claims.FirstOrDefault(x => x.Type == UserClaims.Id)?.Value}") ?? "");
            if (permissions != null && permissions.Any(x => x == requirement.Permission))
            {
                context.Succeed(requirement);
                return;
            }
            context.Fail();
        }
    }
}