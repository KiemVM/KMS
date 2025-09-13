using AutoMapper;
using Microsoft.EntityFrameworkCore;
using KMS.Core.Entities.Identity;
using KMS.Core.ViewModels.ConfigOptions;
using KMS.Common.Constants;
using KMS.Common.Validate;
using System.ComponentModel.DataAnnotations;

namespace KMS.Core.ViewModels.Identity
{
    public class AppUserViewModel
    {
        public Guid Id { get; set; }

        [Comment("Tên đăng nhập")]
        [Validate(30)]
        public string? UserName { get; set; }

        [Comment("Họ và tên")]
        [Validate(300)]
        public string? FullName { get; set; }
        [Comment("Chức vụ")]
        [Validate(300)]
        public string? Position { get; set; }
        public int? Sort { get; set; }
        [Comment("Mật khẩu")]
        [Validate(300)]
        public string? PasswordSha256 { get; set; } = "";

        public bool IsActive { get; set; }

        [Comment("Email")]
        public string? Email { get; set; }

        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }

        [Validate(500, false)]
        public string? Avatar { get; set; }

        public DateTime? LastLoginDate { get; set; }

        [Comment("Trạng thái")]
        public UserStatus Status { get; set; }

        [Comment("Người tạo")]
        public Guid? UserIdCreated { get; set; }

        [Comment("Người sửa")]
        public Guid? UserIdModified { get; set; }
        [Comment("Số tài khoản")]
        [Validate(4000, false)]
        public string? BankNumber { get; set; }
        public string? PhoneNumber { get; set; }

        public DateTime? DateCreated { get; set; } = DateTime.Now;
        public DateTime? DateModified { get; set; } = DateTime.Now;
        [Comment("Sinh nhật")]
        public DateTime? Dob { get; set; }

        [UIHint(UiHint.Avatar)]
        public AvatarViewModel? AvatarViewModel { get; set; }

        public List<AppRoleViewModel>? AppRoleViewModels { get; set; }
        [Comment("Loại tài khoản")]
        public UserType? Type { get; set; }
        public static AppUserViewModel GetData(AppUser? appUser)
        {
            return new AppUserViewModel()
            {
                UserName = appUser?.UserName ?? "",
                Avatar = appUser?.Avatar,
                Email = appUser?.Email,
                FullName = appUser?.FullName ?? ""
            };
        }

        public class AutoMapperProfiles : Profile
        {
            public AutoMapperProfiles()
            {
                CreateMap<AppUser, AppUserViewModel>();
            }
        }
    }
    public enum UserType
    {
        [Display(Name = "Tất cả")]
        [CssClass("bg-light-primary")]
        [Badge("badge-primary")]
        All = 0,

        [Display(Name = "Admin")]
        [CssClass("bg-light-success")]
        [Badge("badge-success")]
        Admin = 1,

        [Display(Name = "User")]
        [CssClass("bg-light-primary")]
        [Badge("badge-primary")]
        User = 2,
    }

    public enum UserStatus
    {
        [Display(Name = "Mới tạo")]
        [CssClass("")]
        [Badge("badge-light-primary")]
        Create = 1,

        [Display(Name = "Đã xóa")]
        [CssClass("bg-light")]
        [Badge("badge-light-danger")]
        Delete = 2,
    }

    public static class UserMapper
    {
        public static void UpdateViewModel(this AppUser user, AppUserViewModel userViewModel)
        {
            user.FullName = userViewModel.FullName;
            user.UserName = userViewModel.UserName;
            user.PasswordSha256 = userViewModel.PasswordSha256;
            user.Avatar = userViewModel.Avatar;
            user.Email = userViewModel.Email;
            user.PhoneNumber = userViewModel.PhoneNumber;
            user.BankNumber = userViewModel.BankNumber;
            user.Dob = userViewModel.Dob;
            user.Position = userViewModel.Position;

            user.NormalizedUserName = userViewModel.UserName?.ToUpper();
            user.NormalizedEmail = userViewModel?.Email?.ToUpper();
        }
    }

    public static class UserValidate
    {
        public static List<string> Validate(AppUserViewModel userViewModel)
        {
            List<string> msgs = userViewModel.Validate();
            return msgs;
        }
    }

    public class AvatarViewModel
    {
        public string? Avatar { get; set; }
        public string? UserName { get; set; }
        public string? FullName { get; set; }
        public bool HasAvatar => !string.IsNullOrWhiteSpace(Avatar);

        public static AvatarViewModel UpdateAvatar(AppUser appUser)
        {
            return new AvatarViewModel()
            {
                UserName = appUser?.UserName,
                Avatar = appUser?.Avatar ?? "/images/avatar.png",
                FullName = appUser?.FullName
            };
        }
    }

    public class LoginViewModel
    {
        [Validate(20)]
        public string? UserName { get; set; }

        public string? Password { get; set; }
    }

    public class ChangePasswordViewModel
    {
        public string? PasswordOld { set; get; }
        public string? PasswordNew { set; get; }
    }

    public class EmployeeRoleViewModel
    {
        public string? RoleName { set; get; }
        public int EmployeeId { get; set; }
    }

    public class AuthenticatedResult
    {
        public string? Token { get; set; }
        public string? RefreshToken { get; set; }
        public string? Roles { get; set; }
        public string? Permissions { get; set; }
    }

    public class DataViewModel
    {
        public string? Id { set; get; }

        public string? Value { set; get; }
    }
}