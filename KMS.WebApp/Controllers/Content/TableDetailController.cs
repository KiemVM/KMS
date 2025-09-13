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
    public class TableDetailController : ApiControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public TableDetailController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost("InsertAsync")]
        [Authorize(Permissions.TableDetail.Insert)]
        public async Task<ActionResult> InsertAsync([FromBody] TableDetailViewModel tableDetailViewModel)
        {
            //Bước 1: Khởi tạo các giá trị mặc định
            tableDetailViewModel.UserIdCreated = User.GetUserId();
            //Bước 2: Validate dữ liệu đầu vào (Lấy từ ViewModel ra)
            var msgs = TableDetailValidate.Validate(tableDetailViewModel);
            if (msgs.Count > 0) return BadRequest(string.Join("</br>", msgs));
            //Bước 3: Thực hiện nghiệp vụ
            tableDetailViewModel.UserIdCreated = User.GetUserId();
            tableDetailViewModel = await _unitOfWork.TableDetailRepository.InsertAsync(tableDetailViewModel);
            if (tableDetailViewModel == new TableDetailViewModel()) return NotFound();
            //Bước 4: Ghi lịch sử
            await _unitOfWork.LogHistoryRepository.InsertAsync(new LogHistory()
            {
                UserId = User.GetUserId(),
                ObjectId = tableDetailViewModel.Id,
                Action = ConstSystem.Insert,
                Content = "Thêm mới đối tượng",
            });
            await _unitOfWork.SaveAsync();
            return Ok();
        }

        [HttpPut("UpdateAsync/{id}")]
        [Authorize(Permissions.TableDetail.Update)]
        public async Task<ActionResult> UpdateAsync(Guid id, [FromBody] TableDetailViewModel tableDetailViewModel)
        {
            //Bước 1: Khởi tạo các giá trị mặc định
            tableDetailViewModel.UserIdModified = User.GetUserId();
            //Bước 2: Validate dữ liệu đầu vào (Lấy từ ViewModel ra)
            var msgs = TableDetailValidate.Validate(tableDetailViewModel);
            if (msgs.Count > 0) return BadRequest(string.Join("</br>", msgs));
            //Bước 3: Thực hiện nghiệp vụ
            tableDetailViewModel.UserIdModified = User.GetUserId();
            tableDetailViewModel = await _unitOfWork.TableDetailRepository.UpdateAsync(tableDetailViewModel);
            if (tableDetailViewModel == new TableDetailViewModel()) return NotFound();
            //Bước 4: Ghi lịch sử
            await _unitOfWork.LogHistoryRepository.InsertAsync(new LogHistory()
            {
                UserId = User.GetUserId(),
                ObjectId = tableDetailViewModel.Id,
                Url = Request.Path.Value ?? "/",
                Action = ConstSystem.Update,
                Content = "Cập nhật đối tượng",
            });
            await _unitOfWork.SaveAsync();
            return Ok();
        }

        [HttpGet("{id}")]
        [Authorize(Permissions.TableDetail.View)]
        public async Task<ActionResult> FindByIdAsync(Guid id)
        {
            var tableDetailViewModel = await _unitOfWork.TableDetailRepository.FindByIdAsync(id);
            return Ok(tableDetailViewModel);
        }

        [HttpDelete("RemoveAsync/{id}")]
        [Authorize(Permissions.TableDetail.Remove)]
        public async Task<ActionResult> RemoveAsync(Guid id)
        {
            await _unitOfWork.TableDetailRepository.RemoveAsync(id);
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
        [Authorize(Permissions.TableDetail.View)]
        public async Task<ActionResult> Select(string? q = "")
        {
            List<ComboboxViewModel> comboboxViewModels = await _unitOfWork.TableDetailRepository.SelectAsync(q);
            return Ok(comboboxViewModels);
        }
    }
}
