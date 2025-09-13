using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KMS.Core.Entities.Cache
{
    [Table("DatabaseCache")]
    public class DatabaseCache
    {
        [Key]
        [DataType("nvarchar(499)")]
        public string? Id { set; get; }

        [DataType("varbinary(max)")]
        public byte[]? Value { set; get; }

        public DateTimeOffset? ExpiresAtTime { set; get; }

        public long? SlidingExpirationInSeconds { set; get; }

        public DateTimeOffset? AbsoluteExpiration { set; get; }
    }
}