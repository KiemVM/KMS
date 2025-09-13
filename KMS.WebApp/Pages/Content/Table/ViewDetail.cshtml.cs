using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using KMS.Common.Constants;
using KMS.Core.ViewModels.Content;
using KMS.Data.SeedWorks;
using KMS.WebApp.Extensions;

namespace KMS.WebApp.Pages.Content.Table
{
    [Authorize(Permissions.Table.ViewDetail)]
    public class ViewDetailModel : PageModelBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public ViewDetailModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [BindProperty(SupportsGet = true)] public Guid Id { get; set; } = Guid.Empty;

        public TableViewModel TableViewModel { get; set; } = new TableViewModel();
        public string Title => $"Chi tiết {TableViewModel?.Name}";

        public async Task OnGetAsync()
        {
            TableViewModel = await _unitOfWork.TableRepository.FindByIdAsync(Id);
        }
    }
}
