using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using KMS.Core.ViewModels.Content;
using KMS.Common.Validate;
using KMS.Core.AbstractClass;
using Microsoft.EntityFrameworkCore;

namespace KMS.Core.Entities.Content;
[Comment("Quản lý danh sách các ứng dụng")]
[Table("Application")]
public class Application : EntityExample
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [Comment("Tên ứng dụng")]
    [Validate]
    public string? Name { get; set; }

    [Comment("Mã")]
    public string? Code { get; set; }

    [Comment("Mô tả")]
    public string? Description { get; set; }

    [Comment("Hình ảnh")]
    public string? Image { get; set; }

    [Comment("Công ty")]
    public Guid CompanyId { get; set; } = Guid.Empty;

    [Comment("Trạng thái")]
    public ApplicationStatus Status { get; set; } = ApplicationStatus.Create;
}