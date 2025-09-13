using Microsoft.EntityFrameworkCore;
using KMS.Common.Validate;
using KMS.Core.AbstractClass;
using KMS.Core.ViewModels.Content;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace KMS.Core.Entities.Content
{
    [Comment("Quản lý danh sách các Schema")]
    [Table("Schema")]
    public class Schema: EntityExample
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(500)]
        public string Name { get; set; } = string.Empty;

        [Comment("Mô tả")]
        public string? Description { get; set; }

        [MaxLength(500)]
        public string Slug { get; set; } = string.Empty;

        [Comment("Ứng dụng")]
        public Guid ApplicationId { get; set; }
        [Comment("Thông tin DB")]
        public string Database { get; set; }

        [Comment("Trạng thái")]
        public SchemaStatus Status { get; set; } = SchemaStatus.Create;

        [Comment("Vị trí")]
        public string Location { get; set; } = string.Empty;
    }
}