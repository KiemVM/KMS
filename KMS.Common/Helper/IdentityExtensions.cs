using Newtonsoft.Json;
using KMS.Common.Constants;
using System.Security.Claims;

namespace KMS.Common.Helper
{
    public static class IdentityExtensions
    {
        public static string GetEmail(this ClaimsPrincipal principal)
        {
            var value = principal.FindFirst(UserClaims.Email)?.Value;
            return value ?? String.Empty;
        }

        public static string GetAvatar(this ClaimsPrincipal principal)
        {
            var value = principal.FindFirst(UserClaims.Avatar)?.Value;
            if (string.IsNullOrWhiteSpace(value)) return "/images/avatar.png";
            return value;
        }

        public static string GetUserName(this ClaimsPrincipal principal)
        {
            var value = principal.FindFirst(UserClaims.UserName)?.Value;
            return value ?? String.Empty;
        }

        public static string GetFullName(this ClaimsPrincipal principal)
        {
            var value = principal.FindFirst(UserClaims.FullName)?.Value;
            return value ?? String.Empty;
        }

        public static bool CheckPermissions(this ClaimsPrincipal principal, string permission)
        {
            try
            {
                var value = principal.FindFirst(UserClaims.Permissions)?.Value;
                if (string.IsNullOrWhiteSpace(value)) return false;
                if (principal.CheckRole(ConstSystem.RoleAdmin)) return true;
                var permissions = JsonConvert.DeserializeObject<List<string>>(value);
                return permissions != null && permissions.Any(x => x == permission);
            }
            catch
            {
                return false;
            }
        }

        public static bool CheckRole(this ClaimsPrincipal principal, string? role = "")
        {
            try
            {
                var value = principal.FindFirst(UserClaims.Roles)?.Value;
                if (string.IsNullOrWhiteSpace(value)) return false;
                if (string.IsNullOrWhiteSpace(role)) return false;

                var roles = JsonConvert.DeserializeObject<List<string>>(value) ?? new List<string>();
                return roles.Any(x => x == role);
            }
            catch
            {
                return false;
            }
        }

        public static bool IsAdmin(this ClaimsPrincipal principal)
        {
            return principal.CheckRole(ConstSystem.RoleAdmin);
        }

        public static Guid GetUserId(this ClaimsPrincipal principal)
        {
            var id = principal.FindFirst(UserClaims.Id)?.Value;
            if (string.IsNullOrEmpty(id))
                return Guid.Empty;
            return Guid.Parse(id);
        }
    }
}