using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using KMS.Common.Constants;
using KMS.Core.ViewModels.Content;
using KMS.Data.SeedWorks;
using KMS.WebApp.Extensions;

namespace KMS.WebApp.Pages.Content.Company
{
    [Authorize(Permissions.Company.ViewDetail)]
    public class ViewDetailModel : PageModelBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public ViewDetailModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [BindProperty(SupportsGet = true)] public Guid Id { get; set; } = Guid.Empty;

        public CompanyViewModel CompanyViewModel { get; set; } = new CompanyViewModel();
        public string Title => $"Chi tiết {CompanyViewModel?.Name}";

        public async Task OnGetAsync()
        {
            CompanyViewModel = await _unitOfWork.CompanyRepository.FindByIdAsync(Id);
        }
    }
}
