using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using KMS.Core.Interfaces;
using KMS.Core.ViewModels.Identity;
using KMS.Common.Validate;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KMS.Core.Entities.Identity
{
    [Table("AppUsers")]
    public class AppUser : IdentityUser<Guid>, ILogAction
    {
        [Comment("Tên hiển thị")]
        [Validate(300)]
        public string? FullName { get; set; }

        [Comment("Mật khẩu")]
        [Validate(300)]
        public string? PasswordSha256 { get; set; } = "/";
        [Comment("Loại tài khoản")]
        public UserType? Type { get; set; }

        [Comment("Trạng thái")]
        public UserStatus Status { get; set; }

        [Comment("Người tạo")]
        public Guid? UserIdCreated { get; set; }

        [Comment("Người sửa")]
        public Guid? UserIdModified { get; set; }

        [Comment("Ghi chú")]
        [Validate(4000, false)]
        public string? Note { get; set; }

        [Comment("Số tài khoản")]
        [Validate(4000, false)]
        public string? BankNumber { get; set; }

        public DateTime? DateCreated { get; set; } = DateTime.Now;
        public DateTime? DateModified { get; set; } = DateTime.Now;
        public bool IsActive { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }

        [MaxLength(500)]
        public string? Avatar { get; set; }
        [Comment("Sinh nhật")]
        public DateTime? Dob { get; set; }
        public DateTime? LastLoginDate { get; set; }
        [Comment("Chức vụ")]
        [Validate(300)]
        public string? Position { get; set; }
        public int? Sort { get; set; }
    }
}