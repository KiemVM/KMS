using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using KMS.Common.Helper;
using KMS.Common.Models;
using KMS.Core.ViewModels.ConfigOptions;
using KMS.Core.ViewModels.Identity;
using KMS.Data.SeedWorks;

namespace KMS.WebApp.Pages.Identity.AppUser
{
    public class AddOrUpdateModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddOrUpdateModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [BindProperty(SupportsGet = true)] public Guid Id { get; set; } = Guid.Empty;
        [BindProperty(SupportsGet = true)] public UserType? Type { get; set; }

        public AppUserViewModel UserViewModel { get; set; } = new AppUserViewModel();

        public bool IsInsert => Id == Guid.Empty;
        public string Title => IsInsert ? "Thêm tài khoản" : "Sửa tài khoản " + UserViewModel.UserName;
        public string? UrlAjax => IsInsert ? "/Api/User/InsertAsync" : "/Api/User/UpdateAsync/" + Id;

        public List<InputFormViewModel> InputFormViewModels = new List<InputFormViewModel>();

        public InputFormViewModel InputSelect(string url, string id)
        {
            var data = InputFormViewModels.FirstOrDefault(x => x.Id == id) ?? new InputFormViewModel();
            data.Url = url ?? $"/Api/{id}/SelectAsync";
            return data;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            UserViewModel = await _unitOfWork.UserRepository.FindByIdAsync(Id);
            if (!IsInsert)
            {
                UserViewModel.PasswordSha256 = UserViewModel?.PasswordSha256?.DecryptString();
                if (UserViewModel != null) Type = UserViewModel.Type;
            }
            InputFormViewModels = UserViewModel.GetInputForms();
            return Page();
        }
    }
}