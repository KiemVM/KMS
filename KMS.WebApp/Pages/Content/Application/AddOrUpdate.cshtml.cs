using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using KMS.Common.Constants;
using KMS.Common.Models;
using KMS.Core.ViewModels.ConfigOptions;
using KMS.Core.ViewModels.Content;
using KMS.Data.SeedWorks;
using KMS.WebApp.Extensions;

namespace KMS.WebApp.Pages.Content.Application
{
    public class AddOrUpdateModel : PageModelBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddOrUpdateModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [BindProperty(SupportsGet = true)] public Guid Id { get; set; } = Guid.Empty;
        [BindProperty(SupportsGet = true)] public Guid CompanyId { get; set; } = Guid.Empty;
        public ApplicationViewModel ApplicationViewModel { get; set; } = new ApplicationViewModel();

        public bool IsInsert => Id == Guid.Empty;
        public string Title => IsInsert ? $"Thêm {ConstTitle.Application}" : $"Sửa {ConstTitle.Application} {ApplicationViewModel.Number}";
        public string? UrlAjax => IsInsert ? "/Api/Application/InsertAsync" : "/Api/Application/UpdateAsync/" + Id;

        public List<InputFormViewModel> InputFormViewModels = new List<InputFormViewModel>();

        public InputFormViewModel InputSelect(string url, string id)
        {
            var data = InputFormViewModels.FirstOrDefault(x => x.Id == id) ?? new InputFormViewModel();
            data.Url = url ?? $"/Api/{id}/SelectAsync";
            return data;
        }

        public async Task OnGetAsync()
        {
            var applicationViewModel = await _unitOfWork.ApplicationRepository.FindByIdAsync(Id);
            ApplicationViewModel = applicationViewModel;
            if (ApplicationViewModel.CompanyId == Guid.Empty)
                ApplicationViewModel.CompanyId = CompanyId;
            InputFormViewModels = ApplicationViewModel.GetInputForms();
        }
    }
}
