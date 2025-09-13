using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KMS.Common.Constants;
using KMS.Common.Helper;
using KMS.Core.Entities.Log;
using KMS.Core.ViewModels.Content;
using KMS.Core.ViewModels.Extension;
using KMS.Data.SeedWorks;
using KMS.WebApp.Extensions;

namespace KMS.WebApp.Controllers.Content
{
    public class ApplicationController : ApiControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public ApplicationController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost("InsertAsync")]
        [Authorize(Permissions.Application.Insert)]
        public async Task<ActionResult> InsertAsync([FromBody] ApplicationViewModel applicationViewModel)
        {
            //Bước 1: Khởi tạo các giá trị mặc định
            applicationViewModel.UserIdCreated = User.GetUserId();
            //Bước 2: Validate dữ liệu đầu vào (Lấy từ ViewModel ra)
            var msgs = ApplicationValidate.Validate(applicationViewModel);
            if (msgs.Count > 0) return BadRequest(string.Join("</br>", msgs));
            //Bước 3: Thực hiện nghiệp vụ
            applicationViewModel.UserIdCreated = User.GetUserId();
            applicationViewModel = await _unitOfWork.ApplicationRepository.InsertAsync(applicationViewModel);
            if (applicationViewModel == new ApplicationViewModel()) return NotFound();
            //Bước 4: Ghi lịch sử
            await _unitOfWork.LogHistoryRepository.InsertAsync(new LogHistory()
            {
                UserId = User.GetUserId(),
                ObjectId = applicationViewModel.Id,
                Action = ConstSystem.Insert,
                Content = "Thêm mới đối tượng",
            });
            await _unitOfWork.SaveAsync();
            return Ok();
        }

        [HttpPut("UpdateAsync/{id}")]
        [Authorize(Permissions.Application.Update)]
        public async Task<ActionResult> UpdateAsync(Guid id, [FromBody] ApplicationViewModel applicationViewModel)
        {
            //Bước 1: Khởi tạo các giá trị mặc định
            applicationViewModel.UserIdModified = User.GetUserId();
            //Bước 2: Validate dữ liệu đầu vào (Lấy từ ViewModel ra)
            var msgs = ApplicationValidate.Validate(applicationViewModel);
            if (msgs.Count > 0) return BadRequest(string.Join("</br>", msgs));
            //Bước 3: Thực hiện nghiệp vụ
            applicationViewModel.UserIdModified = User.GetUserId();
            applicationViewModel = await _unitOfWork.ApplicationRepository.UpdateAsync(applicationViewModel);
            if (applicationViewModel == new ApplicationViewModel()) return NotFound();
            //Bước 4: Ghi lịch sử
            await _unitOfWork.LogHistoryRepository.InsertAsync(new LogHistory()
            {
                UserId = User.GetUserId(),
                ObjectId = applicationViewModel.Id,
                Url = Request.Path.Value ?? "/",
                Action = ConstSystem.Update,
                Content = "Cập nhật đối tượng",
            });
            await _unitOfWork.SaveAsync();
            return Ok();
        }

        [HttpGet("{id}")]
        [Authorize(Permissions.Application.View)]
        public async Task<ActionResult> FindByIdAsync(Guid id)
        {
            var applicationViewModel = await _unitOfWork.ApplicationRepository.FindByIdAsync(id);
            return Ok(applicationViewModel);
        }

        [HttpDelete("RemoveAsync/{id}")]
        [Authorize(Permissions.Application.Remove)]
        public async Task<ActionResult> RemoveAsync(Guid id)
        {
            await _unitOfWork.ApplicationRepository.RemoveAsync(id);
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
        [Authorize(Permissions.Application.View)]
        public async Task<ActionResult> Select(string? q = "")
        {
            List<ComboboxViewModel> comboboxViewModels = await _unitOfWork.ApplicationRepository.SelectAsync(q);
            return Ok(comboboxViewModels);
        }

        [HttpGet("SelectTreeByType")]
        [Authorize(Permissions.Application.View)]
        public async Task<ActionResult> SelectTreeByType(string? q = "")
        {
            List<ComboboxTreeViewModel> comboboxTreeViewModels = await _unitOfWork.ApplicationRepository.SelectTreeByTypeAsync(q);
            return Ok(comboboxTreeViewModels);
        }

        [HttpGet]
        [Route("GetCompanyInfoByApplicationId/{applicationId}")]
        public async Task<IActionResult> GetCompanyInfoByApplicationId(Guid applicationId)
        {
            // Lấy Application
            var app = await _unitOfWork.ApplicationRepository.FindByIdAsync(applicationId);
            if (app == null || app.CompanyId == Guid.Empty)
                return Ok(new { success = false });

            // Lấy Company
            var company = await _unitOfWork.CompanyRepository.FindByIdAsync(app.CompanyId);
            if (company == null)
                return Ok(new { success = false });

            return Ok(new
            {
                success = true,
                company = new
                {
                    name = company.Name,
                    address = company.Address,
                    email = company.Email,
                    phone = company.Phone
                }
            });
        }
    }
}
