using KMS.Common.Constants;
using KMS.Core.AbstractClass;
using KMS.Core.Entities.Content;
using KMS.Core.ViewModels.Identity;
using System.ComponentModel.DataAnnotations;
using KMS.Common.Validate;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using KMS.Core.ViewModels.ConfigOptions;

namespace KMS.Core.ViewModels.Content
{
    public class SchemaViewModel : EntityExample
    {
       [Comment("Quản lý danh sách các Schema")]
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
        public string? ApplicationName { get; set; } = string.Empty;
        public ApplicationStatus? ApplicationStatus { get; set; } = Content.ApplicationStatus.Create;
        public string Database { get; set; }
		[Comment("Trạng thái")]
		public SchemaStatus Status { get; set; } = SchemaStatus.Create;
		[Comment("Vị trí")]
		public string Location { get; set; } = string.Empty;

        public List<TableViewModel> Tables { get; set; } = new List<TableViewModel>();
        public Guid CompanyId { get; set; } = Guid.Empty;
        public string? CompanyName { get; set; } = String.Empty;
        public CompanyStatus? CompanyStatus { get; set; } = Content.CompanyStatus.Create;

        [UIHint(UiHint.Avatar)]
        public AvatarViewModel? AvatarViewModel { get; set; }

        public class AutoMapperProfiles : Profile
        {
            public AutoMapperProfiles()
            {
                CreateMap<Schema, SchemaViewModel>();
            }
        }
    }
    public enum SchemaStatus
    {
        [Display(Name = "Tất cả")]
        [CssClass("bg-light-primary")]
        [Badge("badge-primary")]
        All = 0,

        [Display(Name = "Mới tạo")]
        [CssClass("bg-light-primary")]
        [Badge("badge-primary")]
        Create = 1,

        [Display(Name = "Đã xóa")]
        [CssClass("bg-light-danger")]
        [Badge("badge-danger")]
        Delete = 2
    }

    /// <summary>
    /// Lớp này dùng để Mapper dữ liệu từ ViewModel sang Model
    /// Cấu trúc {Tên}Mapper
    /// </summary>
    public static class SchemaMapper
    {
        		public static void UpdateViewModel(this Schema schema, SchemaViewModel schemaViewModel)
		{
			schema.Name = schemaViewModel.Name;
			schema.Description = schemaViewModel.Description;
			schema.Slug = schemaViewModel.Slug;
			schema.ApplicationId = schemaViewModel.ApplicationId;
			schema.Database = schemaViewModel.Database;
			schema.Status = schemaViewModel.Status;
			schema.Location = schemaViewModel.Location;
		}

    }

    public static class SchemaConst
    {
       public const string PreNumber = "SCHEMA-";
    }

    public static class SchemaValidate
    {
        public static List<string> Validate(SchemaViewModel schemaViewModel)
        {
            List<string> msgs = schemaViewModel.Validate();
            //Validate phức tạp thì viết ở đây
            return msgs;
        }
    }
}
