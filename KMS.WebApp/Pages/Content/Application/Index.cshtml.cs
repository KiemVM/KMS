using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using KMS.Common.Constants;
using KMS.Common.Helper;
using KMS.Common.Models;
using KMS.Core.ViewModels;
using KMS.Core.ViewModels.Content;
using KMS.Data.SeedWorks;
using KMS.WebApp.Extensions;
using CompanyViewModel = KMS.Core.ViewModels.Content.CompanyViewModel;

namespace KMS.WebApp.Pages.Content.Application
{
    [Authorize(Permissions.Application.View)]
    public class IndexModel : PageModelBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public PagedResult<ApplicationViewModel>? PagedResult;

        public IndexModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [BindProperty(SupportsGet = true)] public int PageIndex { get; set; } = 1;
        [BindProperty(SupportsGet = true)] public string Search { get; set; } = String.Empty;
        [BindProperty(SupportsGet = true)] public int PageSize { get; set; } = 10;
        [BindProperty(SupportsGet = true)] public Guid CompanyId { get; set; } = Guid.Empty;
        [BindProperty(SupportsGet = true)] public Guid KeyId { get; set; } = Guid.Empty;
        [BindProperty(SupportsGet = true)] public ApplicationStatus Status { get; set; } = ApplicationStatus.Create;

        public ButtonViewModel ViewDetailRole(ApplicationViewModel applicationViewModel)
        {
            return new ButtonViewModel()
            {
                Id = applicationViewModel.Id,
                Text = applicationViewModel.Number,
                IsRole = User.CheckPermissions(Permissions.Application.ViewDetail)
            };
        }

        public ButtonViewModel InsertRole()
        {
            var buttonViewModel = new ButtonViewModel();
            if (User.CheckPermissions(Permissions.Application.Insert))
                if (User.CheckPermissions(Permissions.Company.Insert))
                    buttonViewModel = new ButtonViewModel()
                    {
                        Id = Guid.NewGuid(),
                        Url = "/Application/AddOrUpdate"
                    };
            return buttonViewModel;
        }

        public ButtonViewModel UpdateRole(ApplicationViewModel applicationViewModel)
        {
            var buttonViewModel = new ButtonViewModel();
            if (User.CheckPermissions(Permissions.Application.Update))
                buttonViewModel = new ButtonViewModel()
                {
                    Id = applicationViewModel.Id,
                    Url = "/Application/AddOrUpdate?Id=" + applicationViewModel.Id
                };
            return buttonViewModel;
        }

        public ButtonViewModel RemoveRole(ApplicationViewModel applicationViewModel)
        {
            var buttonViewModel = new ButtonViewModel();
            if (User.CheckPermissions(Permissions.Application.Remove))
                buttonViewModel = new ButtonViewModel() { Id = applicationViewModel.Id, Text = applicationViewModel.Number, ControllerName = "Application" };
            return buttonViewModel;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            PagedResult = await _unitOfWork.ApplicationRepository.PagingAsync(PageIndex, PageSize, Search, Status, CompanyId, KeyId);
            return Page();
        }
    }
}
