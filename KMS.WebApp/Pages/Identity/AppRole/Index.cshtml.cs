using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using KMS.Common.Constants;
using KMS.Common.Helper;
using KMS.Common.Models;
using KMS.Core.ViewModels;
using KMS.Core.ViewModels.Identity;
using KMS.Data.SeedWorks;
using KMS.WebApp.Extensions;

namespace KMS.WebApp.Pages.Identity.AppRole
{
    [Authorize(Permissions.Role.View)]
    public class IndexModel : PageModelBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public PagedResult<AppRoleViewModel>? PagedResult;

        public IndexModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [BindProperty(SupportsGet = true)] public int PageIndex { get; set; } = 1;
        [BindProperty(SupportsGet = true)] public string Search { get; set; } = String.Empty;
        [BindProperty(SupportsGet = true)] public int PageSize { get; set; } = 10;

        
        public async Task<IActionResult> OnGetAsync()
        {
            PagedResult = await _unitOfWork.RoleRepository.PagingAsync(PageIndex, PageSize, Search);
            return Page();
        }

        public ButtonViewModel ViewDetailRole(AppRoleViewModel appRoleViewModel)
        {
            return new ButtonViewModel()
            {
                Id = appRoleViewModel.Id,
                Text = appRoleViewModel.Name,
                IsRole = false
            };
        }

        public ButtonViewModel InsertRole()
        {
            var buttonViewModel = new ButtonViewModel();
            if (User.CheckPermissions(Permissions.Role.Insert))
                buttonViewModel = new ButtonViewModel() { Id = Guid.NewGuid() };
            return buttonViewModel;
        }

        public ButtonViewModel UpdateRole(AppRoleViewModel? roleViewModel)
        {
            var buttonViewModel = new ButtonViewModel();
            if (User.CheckPermissions(Permissions.Role.Update))
                buttonViewModel = new ButtonViewModel() { Id = roleViewModel?.Id ?? Guid.Empty, Text = roleViewModel?.Name };
            return buttonViewModel;
        }

        public ButtonViewModel RemoveRole(AppRoleViewModel? roleViewModel)
        {
            var buttonViewModel = new ButtonViewModel();
            if (User.CheckPermissions(Permissions.Role.Remove))
                buttonViewModel = new ButtonViewModel() { Id = roleViewModel?.Id ?? Guid.Empty, Text = roleViewModel?.Name, ControllerName = "Role" };
            return buttonViewModel;
        }
    }
}