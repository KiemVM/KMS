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
    public class SchemaController : ApiControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public SchemaController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost("InsertAsync")]
        [Authorize(Permissions.Schema.Insert)]
        public async Task<ActionResult> InsertAsync([FromBody] SchemaViewModel schemaViewModel)
        {
            //Bước 1: Khởi tạo các giá trị mặc định
            schemaViewModel.UserIdCreated = User.GetUserId();
            //Bước 2: Validate dữ liệu đầu vào (Lấy từ ViewModel ra)
            var msgs = SchemaValidate.Validate(schemaViewModel);
            if (msgs.Count > 0) return BadRequest(string.Join("</br>", msgs));
            //Bước 3: Thực hiện nghiệp vụ
            schemaViewModel.UserIdCreated = User.GetUserId();
            schemaViewModel = await _unitOfWork.SchemaRepository.InsertAsync(schemaViewModel);
            if (schemaViewModel == new SchemaViewModel()) return NotFound();
            //Bước 4: Ghi lịch sử
            await _unitOfWork.LogHistoryRepository.InsertAsync(new LogHistory()
            {
                UserId = User.GetUserId(),
                ObjectId = schemaViewModel.Id,
                Action = ConstSystem.Insert,
                Content = "Thêm mới đối tượng",
            });
            await _unitOfWork.SaveAsync();
            return Ok();
        }

        [HttpPut("UpdateAsync/{id}")]
        [Authorize(Permissions.Schema.Update)]
        public async Task<ActionResult> UpdateAsync(Guid id, [FromBody] SchemaViewModel schemaViewModel)
        {
            //Bước 1: Khởi tạo các giá trị mặc định
            schemaViewModel.UserIdModified = User.GetUserId();
            //Bước 2: Validate dữ liệu đầu vào (Lấy từ ViewModel ra)
            var msgs = SchemaValidate.Validate(schemaViewModel);
            if (msgs.Count > 0) return BadRequest(string.Join("</br>", msgs));
            //Bước 3: Thực hiện nghiệp vụ
            schemaViewModel.UserIdModified = User.GetUserId();
            schemaViewModel = await _unitOfWork.SchemaRepository.UpdateAsync(schemaViewModel);
            if (schemaViewModel == new SchemaViewModel()) return NotFound();
            //Bước 4: Ghi lịch sử
            await _unitOfWork.LogHistoryRepository.InsertAsync(new LogHistory()
            {
                UserId = User.GetUserId(),
                ObjectId = schemaViewModel.Id,
                Url = Request.Path.Value ?? "/",
                Action = ConstSystem.Update,
                Content = "Cập nhật đối tượng",
            });
            await _unitOfWork.SaveAsync();
            return Ok();
        }

        [HttpGet("{id}")]
        [Authorize(Permissions.Schema.View)]
        public async Task<ActionResult> FindByIdAsync(Guid id)
        {
            var schemaViewModel = await _unitOfWork.SchemaRepository.FindByIdAsync(id);
            return Ok(schemaViewModel);
        }

        [HttpDelete("RemoveAsync/{id}")]
        [Authorize(Permissions.Schema.Remove)]
        public async Task<ActionResult> RemoveAsync(Guid id)
        {
            await _unitOfWork.SchemaRepository.RemoveAsync(id);
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
        [Authorize(Permissions.Schema.View)]
        public async Task<ActionResult> Select(string? q = "")
        {
            List<ComboboxViewModel> comboboxViewModels = await _unitOfWork.SchemaRepository.SelectAsync(q);
            return Ok(comboboxViewModels);
        }

        [HttpGet("SelectTreeByType")]
        [Authorize(Permissions.Application.View)]
        public async Task<ActionResult> SelectTreeByType(string? q = "")
        {
            List<ComboboxTreeViewModel> comboboxTreeViewModels = await _unitOfWork.SchemaRepository.SelectTreeByTypeAsync(q);
            return Ok(comboboxTreeViewModels);
        }
    }
}
