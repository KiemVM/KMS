using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using KMS.Common.Constants;
using KMS.Common.Models;
using KMS.Core.ViewModels.ConfigOptions;
using KMS.Core.ViewModels.Content;
using KMS.Data.SeedWorks;
using KMS.WebApp.Extensions;

namespace KMS.WebApp.Pages.Content.Schema
{
    public class AddOrUpdateModel : PageModelBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddOrUpdateModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [BindProperty(SupportsGet = true)] public Guid Id { get; set; } = Guid.Empty;
        public SchemaViewModel SchemaViewModel { get; set; } = new SchemaViewModel();

        public bool IsInsert => Id == Guid.Empty;
        public string Title => IsInsert ? $"Thêm {ConstTitle.Schema}" : $"Sửa {ConstTitle.Schema} {SchemaViewModel.Number}";
        public string? UrlAjax => IsInsert ? "/Api/Schema/InsertAsync" : "/Api/Schema/UpdateAsync/" + Id;

        public List<InputFormViewModel> InputFormViewModels = new List<InputFormViewModel>();

        public InputFormViewModel InputSelect(string url, string id)
        {
            var data = InputFormViewModels.FirstOrDefault(x => x.Id == id) ?? new InputFormViewModel();
            data.Url = url ?? $"/Api/{id}/SelectAsync";
            return data;
        }

        public async Task OnGetAsync()
        {
            var schemaViewModel = await _unitOfWork.SchemaRepository.FindByIdAsync(Id);
            SchemaViewModel = schemaViewModel;
            InputFormViewModels = SchemaViewModel.GetInputForms();
        }
    }
}
