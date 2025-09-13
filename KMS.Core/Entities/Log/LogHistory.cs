using KMS.Core.AbstractClass;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KMS.Core.Entities.Log
{
    [Table("LogHistory")]
    public class LogHistory : DateTracking
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { set; get; }

        public Guid ObjectId { set; get; }
        public Guid? UserId { set; get; }

        [MaxLength(30)]
        public string? Action { set; get; }

        public string? Content { set; get; }
        public string? Url { set; get; }
    }
}