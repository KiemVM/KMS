using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using KMS.Common.Constants;
using KMS.Common.Helper;
using KMS.Core.ViewModels.Identity;
using KMS.Data.SeedWorks;
using KMS.WebApp.Extensions;

namespace KMS.WebApp.Pages.Identity.AppUser
{
    [Authorize(Permissions.User.ViewDetail)]
    public class ViewDetailModel : PageModelBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public ViewDetailModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [BindProperty(SupportsGet = true)] public Guid Id { get; set; } = Guid.Empty;

        public AppUserViewModel AppUserViewModel { get; set; } = new();
        public string Title => $"Chi tiết {AppUserViewModel?.FullName}";

        public async Task OnGetAsync()
        {
            AppUserViewModel = await _unitOfWork.UserRepository.FindByIdAsync(Id);
            AppUserViewModel.PasswordSha256 = TextHelper.DecryptString(AppUserViewModel?.PasswordSha256 ?? "");
        }
    }
}