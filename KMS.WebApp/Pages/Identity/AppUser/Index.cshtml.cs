using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using KMS.Common.Constants;
using KMS.Common.Helper;
using KMS.Common.Models;
using KMS.Core.ViewModels;
using KMS.Core.ViewModels.Identity;
using KMS.Data.SeedWorks;
using KMS.WebApp.Extensions;

namespace KMS.WebApp.Pages.Identity.AppUser
{
    [Authorize(Permissions.User.View)]
    public class IndexModel : PageModelBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public PagedResult<AppUserViewModel>? PagedResult;

        public IndexModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [BindProperty(SupportsGet = true)] public int PageIndex { get; set; } = 1;
        [BindProperty(SupportsGet = true)] public string Search { get; set; } = String.Empty;
        [BindProperty(SupportsGet = true)] public int PageSize { get; set; } = 10;
        [BindProperty(SupportsGet = true)] public UserType UserType { get; set; }


        public async Task<IActionResult> OnGetAsync()
        {
            Guid userId = User.IsAdmin() ? Guid.Empty : User.GetUserId();
            PagedResult = await _unitOfWork.UserRepository.PagingAsync(PageIndex, PageSize, Search, userId, UserType);
            return Page();
        }

        public ButtonViewModel ViewDetailRole(AppUserViewModel userViewModel)
        {
            return new ButtonViewModel()
            {
                Id = userViewModel.Id,
                Text = userViewModel?.UserName,
                IsRole = User.CheckPermissions(Permissions.User.ViewDetail)
            };
        }

        public ButtonViewModel InsertRole()
        {
            var buttonViewModel = new ButtonViewModel();
            if (User.CheckPermissions(Permissions.User.Insert))
                buttonViewModel = new ButtonViewModel() { Id = Guid.NewGuid() };
            return buttonViewModel;
        }

        public ButtonViewModel UpdateRole(AppUserViewModel? userViewModel)
        {
            var buttonViewModel = new ButtonViewModel();
            if (User.CheckPermissions(Permissions.User.Update))
                buttonViewModel = new ButtonViewModel() { Id = userViewModel?.Id ?? Guid.Empty, Text = userViewModel?.UserName };
            return buttonViewModel;
        }

        public ButtonViewModel RemoveRole(AppUserViewModel? userViewModel)
        {
            var buttonViewModel = new ButtonViewModel();
            if (User.CheckPermissions(Permissions.User.Remove))
                buttonViewModel = new ButtonViewModel() { Id = userViewModel?.Id ?? Guid.Empty, Text = userViewModel?.UserName, ControllerName = "User" };
            return buttonViewModel;
        }
    }
}