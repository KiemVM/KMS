using AutoMapper;
using Microsoft.EntityFrameworkCore;
using KMS.Common.Validate;
using KMS.Core.Entities.Identity;
using KMS.Core.ViewModels.ConfigOptions;

namespace KMS.Core.ViewModels.Identity
{
    public class AppRoleViewModel
    {
        public Guid Id { get; set; }

        [Comment("Mã phân quyền")]
        [Validate(100)]
        public string? Name { get; set; }

        [Comment("Tên hiển thị")]
        [Validate(200)]
        public string? DisplayName { get; set; }

        public List<PermissionViewModel>? PermissionViewModels { get; set; }
        public string[]? Permissions { get; set; }

        public class AutoMapperProfiles : Profile
        {
            public AutoMapperProfiles()
            {
                CreateMap<AppRole, AppRoleViewModel>();
            }
        }
    }

    public static class RoleMapper
    {
        public static void UpdateViewModel(this AppRole role, AppRoleViewModel roleViewModel)
        {
            role.DisplayName = roleViewModel.DisplayName;
            role.Name = roleViewModel.Name;
        }
    }

    public static class RoleValidate
    {
        public static List<string> Validate(AppRoleViewModel appRoleViewModel)
        {
            List<string> msgs = appRoleViewModel.Validate();
            return msgs;
        }
    }
}