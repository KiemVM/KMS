using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using KMS.Common.Constants;
using KMS.Core.ViewModels.Content;
using KMS.Data.SeedWorks;
using KMS.WebApp.Extensions;

namespace KMS.WebApp.Pages.Content.Schema
{
    [Authorize(Permissions.Schema.ViewDetail)]
    public class ViewDetailModel : PageModelBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public ViewDetailModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [BindProperty(SupportsGet = true)] public Guid Id { get; set; } = Guid.Empty;

        public SchemaViewModel SchemaViewModel { get; set; } = new SchemaViewModel();
        public string Title => $"Chi tiết {SchemaViewModel?.Name}";

        public async Task OnGetAsync()
        {
            SchemaViewModel = await _unitOfWork.SchemaRepository.FindByIdAsync(Id);
        }
    }
}
