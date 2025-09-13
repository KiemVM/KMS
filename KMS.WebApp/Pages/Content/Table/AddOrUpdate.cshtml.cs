using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using KMS.Common.Constants;
using KMS.Common.Models;
using KMS.Core.ViewModels.ConfigOptions;
using KMS.Core.ViewModels.Content;
using KMS.Data.SeedWorks;
using KMS.WebApp.Extensions;

namespace KMS.WebApp.Pages.Content.Table
{
    public class AddOrUpdateModel : PageModelBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddOrUpdateModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [BindProperty(SupportsGet = true)] public Guid Id { get; set; } = Guid.Empty;
        public TableViewModel TableViewModel { get; set; } = new TableViewModel();

        public bool IsInsert => Id == Guid.Empty;
        public string Title => IsInsert ? $"Thêm {ConstTitle.Table}" : $"Sửa {ConstTitle.Table} {TableViewModel.Number}";
        public string? UrlAjax => IsInsert ? "/Api/Table/InsertAsync" : "/Api/Table/UpdateAsync/" + Id;

        public List<InputFormViewModel> InputFormViewModels = new List<InputFormViewModel>();

        public InputFormViewModel InputSelect(string url, string id)
        {
            var data = InputFormViewModels.FirstOrDefault(x => x.Id == id) ?? new InputFormViewModel();
            data.Url = url ?? $"/Api/{id}/SelectAsync";
            return data;
        }

        public async Task OnGetAsync()
        {
            var tableViewModel = await _unitOfWork.TableRepository.FindByIdAsync(Id);
            TableViewModel = tableViewModel;
            InputFormViewModels = TableViewModel.GetInputForms();
        }
    }
}
