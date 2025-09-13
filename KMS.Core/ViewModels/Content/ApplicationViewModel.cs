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
    public class ApplicationViewModel : EntityExample
    {
        [Comment("Quản lý danh sách các ứng dụng")]
		public Guid Id { get; set; }
		[Comment("Tên ứng dụng")]
		[Validate]
		public string? Name { get; set; }
		[Comment("Mã")]
		public string? Code { get; set; }
		[Comment("Mô tả")]
		public string? Description { get; set; }
		[Comment("Hình ảnh")]
		public string? Image { get; set; }
		[Comment("Công ty")]
		public Guid CompanyId { get; set; } = Guid.Empty;

        public string? CompanyName { get; set; } = String.Empty;
        public CompanyStatus CompanyStatus { get; set; } = CompanyStatus.Create;
        [Comment("Trạng thái")]
		public ApplicationStatus Status { get; set; } = ApplicationStatus.Create;

        public List<KeyViewModel> Keys { get; set; } = new List<KeyViewModel>();

        [UIHint(UiHint.Avatar)]
        public AvatarViewModel? AvatarViewModel { get; set; }

        public class AutoMapperProfiles : Profile
        {
            public AutoMapperProfiles()
            {
                CreateMap<Application, ApplicationViewModel>();
            }
        }
    }
    public enum ApplicationStatus
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
    public static class ApplicationMapper
    {
        		public static void UpdateViewModel(this Application application, ApplicationViewModel applicationViewModel)
		{
			application.Name = applicationViewModel.Name;
			application.Code = applicationViewModel.Code;
			application.Description = applicationViewModel.Description;
			application.Image = applicationViewModel.Image;
			application.CompanyId = applicationViewModel.CompanyId;
			application.Status = applicationViewModel.Status;
		}

    }

    public static class ApplicationConst
    {
       public const string PreNumber = "APPLICATION-";
    }

    public static class ApplicationValidate
    {
        public static List<string> Validate(ApplicationViewModel applicationViewModel)
        {
            List<string> msgs = applicationViewModel.Validate();
            //Validate phức tạp thì viết ở đây
            return msgs;
        }
    }
}
