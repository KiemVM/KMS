using Microsoft.EntityFrameworkCore;
using KMS.Common.Validate;
using KMS.Core.AbstractClass;
using KMS.Core.ViewModels.Content;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KMS.Core.Entities.Content
{
    [Comment("Quản lý danh sách công ty")]
    [Table("Company")]
    public class Company : EntityExample
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Comment("Tên công ty")]
        [Validate]
        public string? Name { get; set; }

        public string? Slug { get; set; }

        [Comment("Mã")]
        public string? Code { get; set; }

        [Comment("Mô tả")]
        public string? Description { get; set; }

        [Comment("Hình ảnh")]
        public string? Image { get; set; }

        [Comment("Địa chỉ")]
        public string? Address { get; set; }

        [Comment("Điện thoại")]
        public string? Phone { get; set; }

        [Comment("Email")]
        public string? Email { get; set; }

        [Comment("Trạng thái")]
        public CompanyStatus Status { get; set; } = CompanyStatus.Create;
    }
}