using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using KMS.Core.AbstractClass;
using KMS.Core.ViewModels.Content;
using Microsoft.EntityFrameworkCore;
using KMS.Core.ViewModels.Content;

namespace KMS.Core.Entities.Content;
[Comment("Quản lý các trường trong table")]
[Table("TableDetail")]
public class TableDetail: EntityExample
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [Comment("Tên trường")]
    public string Name { get; set; } = string.Empty;

    [Comment("Table")]
    public Guid TableId { get; set; } = Guid.Empty;

    [Comment("Trạng thái")]
    public TableDetailStatus Status { get; set; } = TableDetailStatus.Create;

    [Comment("Kiểu dữ liệu")]
    public string DataType { get; set; } = string.Empty;
    [Comment("Mã hóa")]
    public bool IsEncrypt { get; set; } = false;
}