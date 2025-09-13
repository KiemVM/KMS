using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using KMS.Core.AbstractClass;

namespace KMS.Core.Entities.Log
{
    [Table("LogAction")]
    public class LogAction : DateTracking
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { set; get; }

        public Guid LogHistoryId { set; get; }
        public Guid? UserId { set; get; }
        public Guid ObjectId { set; get; }
        public string? ObjectName { get; set; }

        [MaxLength(30)]
        public string? Action { set; get; }

        public string? PropertyName { get; set; }
        public string? OldValue { get; set; }
        public string? NewValue { get; set; }
    }
}