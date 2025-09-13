using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using KMS.Common.Constants;
using KMS.Common.Helper;
using KMS.Common.Models;
using KMS.Core.ViewModels;
using KMS.Core.ViewModels.Content;
using KMS.Data.SeedWorks;
using KMS.WebApp.Extensions;

namespace KMS.WebApp.Pages.Content.Table
{
    [Authorize(Permissions.Table.View)]
    public class IndexModel : PageModelBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public PagedResult<TableViewModel>? PagedResult;

        public IndexModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [BindProperty(SupportsGet = true)] public int PageIndex { get; set; } = 1;
        [BindProperty(SupportsGet = true)] public string Search { get; set; } = String.Empty;
        [BindProperty(SupportsGet = true)] public int PageSize { get; set; } = 10;
        [BindProperty(SupportsGet = true)] public TableStatus Status { get; set; } = TableStatus.All;

        public ButtonViewModel ViewDetailRole(TableViewModel tableViewModel)
        {
            return new ButtonViewModel()
            {
                Id = tableViewModel.Id,
                Text = tableViewModel.Number,
                IsRole = User.CheckPermissions(Permissions.Table.ViewDetail)
            };
        }

        public ButtonViewModel InsertRole()
        {
            var buttonViewModel = new ButtonViewModel();
            if (User.CheckPermissions(Permissions.Table.Insert))
                buttonViewModel = new ButtonViewModel() { Id = Guid.NewGuid() };
            return buttonViewModel;
        }

        public ButtonViewModel UpdateRole(TableViewModel tableViewModel)
        {
            var buttonViewModel = new ButtonViewModel();
            if (User.CheckPermissions(Permissions.Table.Update))
                buttonViewModel = new ButtonViewModel() { Id = tableViewModel.Id, Text = tableViewModel.Number };
            return buttonViewModel;
        }

        public ButtonViewModel RemoveRole(TableViewModel tableViewModel)
        {
            var buttonViewModel = new ButtonViewModel();
            if (User.CheckPermissions(Permissions.Table.Remove))
                buttonViewModel = new ButtonViewModel() { Id = tableViewModel.Id, Text = tableViewModel.Number, ControllerName = "Table" };
            return buttonViewModel;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            PagedResult = await _unitOfWork.TableRepository.PagingAsync(PageIndex, PageSize, Search, Status);
            return Page();
        }
    }
}
