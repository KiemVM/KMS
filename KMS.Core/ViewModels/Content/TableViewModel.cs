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
    public class TableViewModel : EntityExample
    {
       [Comment("Quản lý table của Schema")]
		public Guid Id { get; set; }
		public string Name { get; set; } = String.Empty;
		[Required]
		public Guid SchemaId { get; set; } = Guid.Empty;
		[Comment("Trạng thái")]
        public TableStatus Status { get; set; } = TableStatus.Create;
        
        public List<TableDetailViewModel> TableDetails { get; set; } = new List<TableDetailViewModel>();

        [UIHint(UiHint.Avatar)]
        public AvatarViewModel? AvatarViewModel { get; set; }

        public class AutoMapperProfiles : Profile
        {
            public AutoMapperProfiles()
            {
                CreateMap<Table, TableViewModel>();
            }
        }
    }
    public enum TableStatus
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
    public static class TableMapper
    {
        public static void UpdateViewModel(this Table table, TableViewModel tableViewModel)
		{
			table.Name = tableViewModel.Name;
			table.SchemaId = tableViewModel.SchemaId;
			table.Status = tableViewModel.Status;
		}

    }

    public static class TableConst
    {
       public const string PreNumber = "TABLE-";
    }

    public static class TableValidate
    {
        public static List<string> Validate(TableViewModel tableViewModel)
        {
            List<string> msgs = tableViewModel.Validate();
            //Validate phức tạp thì viết ở đây
            return msgs;
        }
    }
}
