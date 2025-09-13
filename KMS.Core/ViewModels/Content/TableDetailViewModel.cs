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
    public class TableDetailViewModel : EntityExample
    { 
        [Comment("Quản lý các trường trong table")]
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


        [UIHint(UiHint.Avatar)]
        public AvatarViewModel? AvatarViewModel { get; set; }

        public class AutoMapperProfiles : Profile
        {
            public AutoMapperProfiles()
            {
                CreateMap<TableDetail, TableDetailViewModel>();
            }
        }
    }
    public enum TableDetailStatus
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
    public static class TableDetailMapper
    {
        public static void UpdateViewModel(this TableDetail tableDetail, TableDetailViewModel tableDetailViewModel)
		{
			tableDetail.Name = tableDetailViewModel.Name;
			tableDetail.TableId = tableDetailViewModel.TableId;
			tableDetail.Status = tableDetailViewModel.Status;
			tableDetail.DataType = tableDetailViewModel.DataType;
			tableDetail.IsEncrypt = tableDetailViewModel.IsEncrypt;
		}

    }

    public static class TableDetailConst
    {
       public const string PreNumber = "TABLEDETAIL-";
    }

    public static class TableDetailValidate
    {
        public static List<string> Validate(TableDetailViewModel tableDetailViewModel)
        {
            List<string> msgs = tableDetailViewModel.Validate();
            //Validate phức tạp thì viết ở đây
            return msgs;
        }
    }
}
