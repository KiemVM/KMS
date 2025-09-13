using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using KMS.Common.Constants;
using KMS.Common.Helper;
using KMS.Common.Models;
using KMS.Core.ViewModels;
using KMS.Core.ViewModels.Content;
using KMS.Data.SeedWorks;
using KMS.WebApp.Extensions;

namespace KMS.WebApp.Pages.Content.Schema
{
    [Authorize(Permissions.Schema.View)]
    public class IndexModel : PageModelBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public PagedResult<SchemaViewModel>? PagedResult;

        public IndexModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [BindProperty(SupportsGet = true)] public int PageIndex { get; set; } = 1;
        [BindProperty(SupportsGet = true)] public string Search { get; set; } = String.Empty;
        [BindProperty(SupportsGet = true)] public int PageSize { get; set; } = 10;
        [BindProperty(SupportsGet = true)] public SchemaStatus Status { get; set; } = SchemaStatus.Create;
        [BindProperty(SupportsGet = true)] public Guid CompanyId { get; set; } = Guid.Empty;
        [BindProperty(SupportsGet = true)] public Guid ApplicationId { get; set; } = Guid.Empty;

        public ButtonViewModel ViewDetailRole(SchemaViewModel schemaViewModel)
        {
            return new ButtonViewModel()
            {
                Id = schemaViewModel.Id,
                Text = schemaViewModel.Number,
                IsRole = User.CheckPermissions(Permissions.Schema.ViewDetail)
            };
        }

        public ButtonViewModel InsertRole()
        {
            var buttonViewModel = new ButtonViewModel();
            if (User.CheckPermissions(Permissions.Schema.Insert))
                buttonViewModel = new ButtonViewModel()
                {
                    Id = Guid.NewGuid(),
                    Url = "/Schema/AddOrUpdate"
                };
            return buttonViewModel;
        }

        public ButtonViewModel UpdateRole(SchemaViewModel schemaViewModel)
        {
            var buttonViewModel = new ButtonViewModel();
            if (User.CheckPermissions(Permissions.Schema.Update))
                buttonViewModel = new ButtonViewModel()
                {
                    Id = schemaViewModel.Id,
                    Url = "/Schema/AddOrUpdate?Id=" + schemaViewModel.Id
                };
            return buttonViewModel;
        }

        public ButtonViewModel RemoveRole(SchemaViewModel schemaViewModel)
        {
            var buttonViewModel = new ButtonViewModel();
            if (User.CheckPermissions(Permissions.Schema.Remove))
                buttonViewModel = new ButtonViewModel() { Id = schemaViewModel.Id, Text = schemaViewModel.Number, ControllerName = "Schema" };
            return buttonViewModel;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            PagedResult = await _unitOfWork.SchemaRepository.PagingAsync(PageIndex, PageSize, Search, Status, CompanyId, ApplicationId);
            return Page();
        }
    }
}
