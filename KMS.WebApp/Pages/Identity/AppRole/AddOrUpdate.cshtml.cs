using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using KMS.Common.Helper;
using KMS.Common.Models;
using KMS.Core.ViewModels.ConfigOptions;
using KMS.Core.ViewModels.Identity;
using KMS.Data.SeedWorks;
using KMS.WebApp.Extensions;

namespace KMS.WebApp.Pages.Identity.AppRole
{
    public class AddOrUpdateModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddOrUpdateModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [BindProperty(SupportsGet = true)] public Guid Id { get; set; } = Guid.Empty;
        public AppRoleViewModel AppRoleViewModel { get; set; } = new AppRoleViewModel();
        public bool IsInsert => Id == Guid.Empty;
        public string Title => IsInsert ? "Thêm phân quyền" : "Sửa phân quyền " + AppRoleViewModel.DisplayName;
        public string? UrlAjax => IsInsert ? "/Api/Role/InsertAsync" : "/Api/Role/UpdateAsync/" + Id;

        public List<InputFormViewModel> InputFormViewModels = new List<InputFormViewModel>();


        public async Task OnGetAsync()
        {
            var appRoleViewModel = await _unitOfWork.RoleRepository.FindByIdAsync(Id);
            appRoleViewModel.PermissionViewModels = await _unitOfWork.RoleRepository.GetPermissionsByRole(Id);
            AppRoleViewModel = appRoleViewModel;
            InputFormViewModels = AppRoleViewModel.GetInputForms();
        }
    }
}