using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using KMS.Common.Constants;
using KMS.Core.ViewModels.Content;
using KMS.Data.SeedWorks;
using KMS.WebApp.Extensions;

namespace KMS.WebApp.Pages.Content.Application
{
    [Authorize(Permissions.Application.ViewDetail)]
    public class ViewDetailModel : PageModelBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public ViewDetailModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [BindProperty(SupportsGet = true)] public Guid Id { get; set; } = Guid.Empty;

        public ApplicationViewModel ApplicationViewModel { get; set; } = new ApplicationViewModel();
        public string Title => $"Chi tiết {ApplicationViewModel?.Name}";

        public async Task OnGetAsync()
        {
            ApplicationViewModel = await _unitOfWork.ApplicationRepository.FindByIdAsync(Id);
        }
    }
}
