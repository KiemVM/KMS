using Microsoft.EntityFrameworkCore;

namespace KMS.Core.AbstractClass
{
    public abstract class SeoMeta : EntityExample
    {
        [Comment("Tiêu đề (Meta SEO)")]
        public string? SeoPageTitle { get; set; }

        [Comment("Từ khóa (Meta SEO)")]
        public string? SeoKeyword { get; set; }

        [Comment("Mô tả (Meta SEO)")]
        public string? SeoDescription { get; set; }

        [Comment("Hình ảnh (Meta SEO)")]
        public string? SeoImages { get; set; }

        [Comment("Slug")]
        public string? Slug { get; set; }
    }
}