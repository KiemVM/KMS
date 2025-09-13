using Microsoft.AspNetCore.Identity;
using KMS.Core.Entities.Identity;
using System.ComponentModel;
using System.Reflection;
using System.Security.Claims;
using KMS.Core.ViewModels.Identity;
using KMS.Common.Constants;

namespace KMS.Data.SeedWorks
{
    public static class ClaimExtensions
    {
        public static List<PermissionViewModel> GetPermissions()
        {
            List<PermissionViewModel> permissionViewModels = new List<PermissionViewModel>();
            var types = typeof(Permissions).GetTypeInfo().DeclaredNestedTypes;
            foreach (var type in types)
            {
                List<RoleClaimsViewModel> roleClaimsViewModels = new List<RoleClaimsViewModel>();
                FieldInfo[] fields = type.GetFields(BindingFlags.Static | BindingFlags.Public);
                foreach (FieldInfo field in fields)
                {
                    var attribute = field.GetCustomAttributes(typeof(DescriptionAttribute), true);
                    string? displayName = (field.GetValue(null) ?? "").ToString();
                    var attributes = field.GetCustomAttributes(typeof(DescriptionAttribute), true);
                    if (attributes.Length > 0)
                    {
                        var description = (DescriptionAttribute)attribute[0];
                        displayName = description.Description;
                    }
                    roleClaimsViewModels.Add(new RoleClaimsViewModel { Value = (field.GetValue(null) ?? "").ToString() ?? "", Type = UserClaims.Permissions, DisplayName = displayName });
                }
                permissionViewModels.Add(new PermissionViewModel()
                {
                    DisplayName = type.GetCustomAttribute<DescriptionAttribute>()?.Description,
                    RoleClaimsViewModels = roleClaimsViewModels
                });
            }

            return permissionViewModels;
        }

        public static async Task AddPermissionClaim(this RoleManager<AppRole> roleManager, AppRole role, string? permission)
        {
            var allClaims = await roleManager.GetClaimsAsync(role);
            if (!allClaims.Any(a => a.Type == UserClaims.Permissions && a.Value == permission))
                if (permission != null)
                    await roleManager.AddClaimAsync(role, new Claim(UserClaims.Permissions, permission));
        }
    }
}