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
    public class TableController : ApiControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public TableController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost("InsertAsync")]
        [Authorize(Permissions.Table.Insert)]
        public async Task<ActionResult> InsertAsync([FromBody] TableViewModel tableViewModel)
        {
            //Bước 1: Khởi tạo các giá trị mặc định
            tableViewModel.UserIdCreated = User.GetUserId();
            //Bước 2: Validate dữ liệu đầu vào (Lấy từ ViewModel ra)
            var msgs = TableValidate.Validate(tableViewModel);
            if (msgs.Count > 0) return BadRequest(string.Join("</br>", msgs));
            //Bước 3: Thực hiện nghiệp vụ
            tableViewModel.UserIdCreated = User.GetUserId();
            tableViewModel = await _unitOfWork.TableRepository.InsertAsync(tableViewModel);
            if (tableViewModel == new TableViewModel()) return NotFound();
            //Bước 4: Ghi lịch sử
            await _unitOfWork.LogHistoryRepository.InsertAsync(new LogHistory()
            {
                UserId = User.GetUserId(),
                ObjectId = tableViewModel.Id,
                Action = ConstSystem.Insert,
                Content = "Thêm mới đối tượng",
            });
            await _unitOfWork.SaveAsync();
            return Ok();
        }

        [HttpPut("UpdateAsync/{id}")]
        [Authorize(Permissions.Table.Update)]
        public async Task<ActionResult> UpdateAsync(Guid id, [FromBody] TableViewModel tableViewModel)
        {
            //Bước 1: Khởi tạo các giá trị mặc định
            tableViewModel.UserIdModified = User.GetUserId();
            //Bước 2: Validate dữ liệu đầu vào (Lấy từ ViewModel ra)
            var msgs = TableValidate.Validate(tableViewModel);
            if (msgs.Count > 0) return BadRequest(string.Join("</br>", msgs));
            //Bước 3: Thực hiện nghiệp vụ
            tableViewModel.UserIdModified = User.GetUserId();
            tableViewModel = await _unitOfWork.TableRepository.UpdateAsync(tableViewModel);
            if (tableViewModel == new TableViewModel()) return NotFound();
            //Bước 4: Ghi lịch sử
            await _unitOfWork.LogHistoryRepository.InsertAsync(new LogHistory()
            {
                UserId = User.GetUserId(),
                ObjectId = tableViewModel.Id,
                Url = Request.Path.Value ?? "/",
                Action = ConstSystem.Update,
                Content = "Cập nhật đối tượng",
            });
            await _unitOfWork.SaveAsync();
            return Ok();
        }

        [HttpGet("{id}")]
        [Authorize(Permissions.Table.View)]
        public async Task<ActionResult> FindByIdAsync(Guid id)
        {
            var tableViewModel = await _unitOfWork.TableRepository.FindByIdAsync(id);
            return Ok(tableViewModel);
        }

        [HttpDelete("RemoveAsync/{id}")]
        [Authorize(Permissions.Table.Remove)]
        public async Task<ActionResult> RemoveAsync(Guid id)
        {
            await _unitOfWork.TableRepository.RemoveAsync(id);
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
        [Authorize(Permissions.Table.View)]
        public async Task<ActionResult> Select(string? q = "")
        {
            List<ComboboxViewModel> comboboxViewModels = await _unitOfWork.TableRepository.SelectAsync(q);
            return Ok(comboboxViewModels);
        }
    }
}
