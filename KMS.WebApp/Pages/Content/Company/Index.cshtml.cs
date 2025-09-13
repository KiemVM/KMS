using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using KMS.Common.Constants;
using KMS.Common.Helper;
using KMS.Common.Models;
using KMS.Core.ViewModels;
using KMS.Core.ViewModels.Content;
using KMS.Data.SeedWorks;
using KMS.WebApp.Extensions;

namespace KMS.WebApp.Pages.Content.Company
{
    [Authorize(Permissions.Company.View)]
    public class IndexModel : PageModelBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public PagedResult<KMS.Core.ViewModels.Content.CompanyViewModel>? PagedResult;

        public IndexModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [BindProperty(SupportsGet = true)] public int PageIndex { get; set; } = 1;
        [BindProperty(SupportsGet = true)] public string Search { get; set; } = String.Empty;
        [BindProperty(SupportsGet = true)] public int PageSize { get; set; } = 10;
        [BindProperty(SupportsGet = true)] public CompanyStatus Status { get; set; } = CompanyStatus.Create;

        public ButtonViewModel ViewDetailRole(KMS.Core.ViewModels.Content.CompanyViewModel companyViewModel)
        {
            return new ButtonViewModel()
            {
                Id = companyViewModel.Id,
                Text = companyViewModel.Number,
                IsRole = User.CheckPermissions(Permissions.Company.ViewDetail)
            };
        }

        public ButtonViewModel InsertRole()
        {
            var buttonViewModel = new ButtonViewModel();
            if (User.CheckPermissions(Permissions.Company.Insert))
                if (User.CheckPermissions(Permissions.Company.Insert))
                    buttonViewModel = new ButtonViewModel()
                    {
                        Id = Guid.NewGuid(),
                        Url = "/Company/AddOrUpdate"
                    };
            return buttonViewModel;
        }

        public ButtonViewModel UpdateRole(KMS.Core.ViewModels.Content.CompanyViewModel companyViewModel)
        {
            var buttonViewModel = new ButtonViewModel();
            if (User.CheckPermissions(Permissions.Company.Update))
                buttonViewModel = new ButtonViewModel()
                {
                    Id = companyViewModel.Id,
                    Url = "/Company/AddOrUpdate?Id=" + companyViewModel.Id
                };
            return buttonViewModel;
        }

        public ButtonViewModel RemoveRole(KMS.Core.ViewModels.Content.CompanyViewModel companyViewModel)
        {
            var buttonViewModel = new ButtonViewModel();
            if (User.CheckPermissions(Permissions.Company.Remove))
                buttonViewModel = new ButtonViewModel() { Id = companyViewModel.Id, Text = companyViewModel.Number, ControllerName = "Company" };
            return buttonViewModel;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            PagedResult = await _unitOfWork.CompanyRepository.PagingAsync(PageIndex, PageSize, Search, Status);
            return Page();
        }
    }
}
