using AutoMapper;
using Microsoft.EntityFrameworkCore;
using KMS.Core.AbstractClass;
using KMS.Core.ViewModels.Content;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace KMS.Core.Entities.Content
{
    [Comment("Quản lý table của Schema")]
    [Table("Table")]
    public class Table : EntityExample
    {
        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; } = String.Empty;

        [Required]
        [MaxLength(200)]
        public Guid SchemaId { get; set; } = Guid.Empty;

        [Comment("Trạng thái")]
        public TableStatus Status { get; set; } = TableStatus.Create;
    }
}