using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace KMS.Core.AbstractClass
{
    public abstract class EntityExample
    {
        [MaxLength(50)]
        [Comment("Mã số")]
        public string? Number { get; set; }

        [Comment("Số thứ tự")]
        public int Count { get; set; }

        [Comment("Người tạo")]
        public Guid? UserIdCreated { get; set; }

        [Comment("Người sửa")]
        public Guid? UserIdModified { get; set; }

        public DateTime? DateCreated { get; set; } = DateTime.Now;
        public DateTime? DateModified { get; set; } = DateTime.Now;
    }
}