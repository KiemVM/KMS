using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using KMS.Common.Constants;
using KMS.Common.Helper;
using KMS.Core.Entities.Identity;
using KMS.Core.Entities.Log;
using KMS.Core.ViewModels.Identity;
using KMS.Data.SeedWorks;
using KMS.WebApp.Extensions;

namespace KMS.WebApp.Controllers.Identity
{
    [AllowAnonymous]
    public class RoleController : ApiControllerBase
    {
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IUnitOfWork _unitOfWork;

        public RoleController(RoleManager<AppRole> roleManager, IUnitOfWork unitOfWork)
        {
            _roleManager = roleManager;
            _unitOfWork = unitOfWork;
        }

        [HttpPost("InsertAsync")]
        //[Authorize(Permissions.Role.Insert)]
        public async Task<ActionResult> InsertAsync([FromBody] AppRoleViewModel roleViewModel)
        {
            //Bước 1: Khởi tạo các giá trị mặc định
            //Bước 2: Validate dữ liệu đầu vào (Lấy từ ViewModel ra)
            var msgs = RoleValidate.Validate(roleViewModel);
            if (msgs.Count > 0) return BadRequest(string.Join("</br>", msgs));

            //Bước 3: Thực hiện nghiệp vụ
            AppRole appRole = new AppRole()
            {
                Id = Guid.NewGuid(),
                Name = roleViewModel.Name,
                DisplayName = roleViewModel.DisplayName,
            };
            await _roleManager.CreateAsync(appRole);
            foreach (var claim in roleViewModel.Permissions ?? new[] { "" })
            {
                if (string.IsNullOrWhiteSpace(claim)) continue;
                await _roleManager.AddPermissionClaim(appRole, claim);
            }
            //Bước 4: Ghi lịch sử
            await _unitOfWork.LogHistoryRepository.InsertAsync(new LogHistory()
            {
                UserId = User.GetUserId(),
                ObjectId = roleViewModel.Id,
                Action = ConstSystem.Insert,
                Content = "Thêm mới phân quyền",
            });
            await _unitOfWork.SaveAsync();
            return Ok();
        }

        [HttpPut("UpdateAsync/{id}")]
        [Authorize(Permissions.Role.Update)]
        public async Task<ActionResult> UpdateAsync(Guid id, [FromBody] AppRoleViewModel roleViewModel)
        {
            //Bước 1: Khởi tạo các giá trị mặc định
            //Bước 2: Validate dữ liệu đầu vào (Lấy từ ViewModel ra)
            var msgs = RoleValidate.Validate(roleViewModel);
            if (msgs.Count > 0) return BadRequest(string.Join("</br>", msgs));
            //Bước 3: Thực hiện nghiệp vụ
            var appRole = await _roleManager.FindByIdAsync(id.ToString());
            if (appRole is null) return BadRequest("Không tồn tại phân quyền này");
            appRole.Name = roleViewModel.Name;
            appRole.DisplayName = roleViewModel.DisplayName;
            var claims = await _roleManager.GetClaimsAsync(appRole);
            foreach (var claim in claims)
            {
                await _roleManager.RemoveClaimAsync(appRole, claim);
            }
            foreach (var claim in roleViewModel.Permissions ?? new[] { "" })
            {
                if (string.IsNullOrWhiteSpace(claim)) continue;
                await _roleManager.AddPermissionClaim(appRole, claim);
            }
            //Bước 4: Ghi lịch sử
            await _unitOfWork.LogHistoryRepository.InsertAsync(new LogHistory()
            {
                UserId = User.GetUserId(),
                ObjectId = roleViewModel.Id,
                Url = Request.Path.Value ?? "/",
                Action = ConstSystem.Update,
                Content = "Cập nhật phân quyền",
            });
            await _unitOfWork.SaveAsync();
            return Ok();
        }
    }
}