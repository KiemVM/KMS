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
    public class CompanyViewModel : EntityExample
    {
        [Comment("Quản lý danh sách công ty")]
		public Guid Id { get; set; }
		[Comment("Tên công ty")]
		[Validate]
		public string? Name { get; set; }
		public string? Slug { get; set; }
		[Comment("Mã")]
		public string? Code { get; set; }
		[Comment("Mô tả")]
		public string? Description { get; set; }
		[Comment("Hình ảnh")]
		public string? Image { get; set; }
		[Comment("Địa chỉ")]
		public string? Address { get; set; }
		[Comment("Điện thoại")]
		public string? Phone { get; set; }
		[Comment("Email")]
		public string? Email { get; set; }
		[Comment("Trạng thái")]
		public CompanyStatus Status { get; set; } = CompanyStatus.Create;
        public List<ApplicationViewModel> Applications { get; set; } = new();
        public List<SchemaViewModel> Schemas { get; set; } = new();

        [UIHint(UiHint.Avatar)]
        public AvatarViewModel? AvatarViewModel { get; set; }

        public class AutoMapperProfiles : Profile
        {
            public AutoMapperProfiles()
            {
                CreateMap<Company, CompanyViewModel>();
            }
        }
    }
    public enum CompanyStatus
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
    public static class CompanyMapper
    {
        		public static void UpdateViewModel(this Company company, CompanyViewModel companyViewModel)
		{
			company.Name = companyViewModel.Name;
			company.Slug = companyViewModel.Slug;
			company.Code = companyViewModel.Code;
			company.Description = companyViewModel.Description;
			company.Image = companyViewModel.Image;
			company.Address = companyViewModel.Address;
			company.Phone = companyViewModel.Phone;
			company.Email = companyViewModel.Email;
			company.Status = companyViewModel.Status;
		}

    }

    public static class CompanyConst
    {
       public const string PreNumber = "COMPANY-";
    }

    public static class CompanyValidate
    {
        public static List<string> Validate(CompanyViewModel companyViewModel)
        {
            List<string> msgs = companyViewModel.Validate();
            //Validate phức tạp thì viết ở đây
            return msgs;
        }
    }
}
