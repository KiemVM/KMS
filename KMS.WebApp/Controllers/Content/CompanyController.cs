using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using KMS.Common.Constants;
using KMS.Common.Helper;
using KMS.Core.Entities.Log;
using KMS.Core.ViewModels.Content;
using KMS.Core.ViewModels.Extension;
using KMS.Data.SeedWorks;
using KMS.WebApp.Extensions;

namespace KMS.WebApp.Controllers.Content
{
    public class CompanyController : ApiControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public CompanyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost("InsertAsync")]
        [Authorize(Permissions.Company.Insert)]
        public async Task<ActionResult> InsertAsync([FromBody] KMS.Core.ViewModels.Content.CompanyViewModel companyViewModel)
        {
            //Bước 1: Khởi tạo các giá trị mặc định
            companyViewModel.UserIdCreated = User.GetUserId();
            //Bước 2: Validate dữ liệu đầu vào (Lấy từ ViewModel ra)
            var msgs = CompanyValidate.Validate(companyViewModel);
            if (msgs.Count > 0) return BadRequest(string.Join("</br>", msgs));
            //Bước 3: Thực hiện nghiệp vụ
            companyViewModel.UserIdCreated = User.GetUserId();
            companyViewModel = await _unitOfWork.CompanyRepository.InsertAsync(companyViewModel);
            if (companyViewModel == new KMS.Core.ViewModels.Content.CompanyViewModel()) return NotFound();
            //Bước 4: Ghi lịch sử
            await _unitOfWork.LogHistoryRepository.InsertAsync(new LogHistory()
            {
                UserId = User.GetUserId(),
                ObjectId = companyViewModel.Id,
                Action = ConstSystem.Insert,
                Content = "Thêm mới đối tượng",
            });
            await _unitOfWork.SaveAsync();
            return Ok();
        }

        [HttpPut("UpdateAsync/{id}")]
        [Authorize(Permissions.Company.Update)]
        public async Task<ActionResult> UpdateAsync(Guid id, [FromBody] KMS.Core.ViewModels.Content.CompanyViewModel companyViewModel)
        {
            //Bước 1: Khởi tạo các giá trị mặc định
            companyViewModel.UserIdModified = User.GetUserId();
            //Bước 2: Validate dữ liệu đầu vào (Lấy từ ViewModel ra)
            var msgs = CompanyValidate.Validate(companyViewModel);
            if (msgs.Count > 0) return BadRequest(string.Join("</br>", msgs));
            //Bước 3: Thực hiện nghiệp vụ
            companyViewModel.UserIdModified = User.GetUserId();
            companyViewModel = await _unitOfWork.CompanyRepository.UpdateAsync(companyViewModel);
            if (companyViewModel == new KMS.Core.ViewModels.Content.CompanyViewModel()) return NotFound();
            //Bước 4: Ghi lịch sử
            await _unitOfWork.LogHistoryRepository.InsertAsync(new LogHistory()
            {
                UserId = User.GetUserId(),
                ObjectId = companyViewModel.Id,
                Url = Request.Path.Value ?? "/",
                Action = ConstSystem.Update,
                Content = "Cập nhật đối tượng",
            });
            await _unitOfWork.SaveAsync();
            return Ok();
        }

        [HttpGet("{id}")]
        [Authorize(Permissions.Company.View)]
        public async Task<ActionResult> FindByIdAsync(Guid id)
        {
            var companyViewModel = await _unitOfWork.CompanyRepository.FindByIdAsync(id);
            return Ok(companyViewModel);
        }

        [HttpDelete("RemoveAsync/{id}")]
        [Authorize(Permissions.Company.Remove)]
        public async Task<ActionResult> RemoveAsync(Guid id)
        {
            await _unitOfWork.CompanyRepository.RemoveAsync(id);
            await _unitOfWork.LogHistoryRepository.InsertAsync(new LogHistory()
            {
                UserId = User.GetUserId(),
                ObjectId = id,
                Url = Request.Path.Value ?? "/",
                Action = ConstSystem.Delete,
                Content = "Xóa đối tượng",
            });
            await _unitOfWork.SaveAsync();
            return Ok();
        }

        [HttpGet("Select")]
        [Authorize(Permissions.Company.View)]
        public async Task<ActionResult> Select(string? q = "")
        {
            List<ComboboxViewModel> comboboxViewModels = await _unitOfWork.CompanyRepository.SelectAsync(q);
            return Ok(comboboxViewModels);
        }

        [HttpGet("SelectTreeByType")]
        [Authorize(Permissions.Company.View)]
        public async Task<ActionResult> SelectTreeByType(string? q = "")
        {
            List<ComboboxTreeViewModel> comboboxTreeViewModels = await _unitOfWork.CompanyRepository.SelectTreeByTypeAsync(q);
            return Ok(comboboxTreeViewModels);
        }
    }
}
