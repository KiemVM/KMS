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
    public class UserController : ApiControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IUnitOfWork _unitOfWork;

        public UserController(IUnitOfWork unitOfWork, UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpPost("InsertAsync")]
        [Authorize(Permissions.User.Insert)]
        public async Task<ActionResult> InsertAsync([FromBody] AppUserViewModel userViewModel)
        {
            //Bước 1: Khởi tạo các giá trị mặc định
            var user = await _userManager.FindByNameAsync((userViewModel.UserName ?? "").Trim());
            if (user != null && userViewModel.Id == Guid.Empty && userViewModel.Status == UserStatus.Create) return BadRequest("Tên đăng nhập này đã tồn tại, vui lòng kiểm tra lại");

            //Bước 2: Validate dữ liệu đầu vào (Lấy từ ViewModel ra)
            var msgs = UserValidate.Validate(userViewModel);
            if (msgs.Count > 0) return BadRequest(string.Join("</br>", msgs));
            //Bước 3: Thực hiện nghiệp vụ
            user = new AppUser();
            user.UpdateViewModel(userViewModel);
            user.Status = UserStatus.Create;
            user.Type = userViewModel.Type;
            user.PasswordSha256 = userViewModel.PasswordSha256.EncryptString();
            user.SecurityStamp = Guid.NewGuid().ToString();
            user.LockoutEnabled = false;
            user.EmailConfirmed = true;
            user.PhoneNumberConfirmed = true;
            user.IsActive = true;
            user.DateCreated = DateTime.Now;
            user.UserIdCreated = User.GetUserId();
            var result = await _userManager.CreateAsync(user, userViewModel?.PasswordSha256 ?? "");

            if (!result.Succeeded) return BadRequest(string.Join("</br>", result.Errors?.Select(x => x.Description) ?? Array.Empty<string>()));

            if (userViewModel?.Type == UserType.Admin && User.IsAdmin())
                await _userManager.AddToRoleAsync(user, ConstSystem.RoleAdmin);
            else
                await _unitOfWork.UserRepository.UpdateRoles(user, userViewModel);

            //Bước 4: Ghi lịch sử
            await _unitOfWork.LogHistoryRepository.InsertAsync(new LogHistory()
            {
                UserId = User.GetUserId(),
                ObjectId = userViewModel.Id,
                Action = ConstSystem.Insert,
                Content = "Thêm mới đối tượng",
            });
            await _unitOfWork.SaveAsync();
            return Ok();
        }

        [HttpPut("UpdateAsync/{id}")]
        [Authorize(Permissions.User.Update)]
        public async Task<ActionResult> UpdateAsync(Guid id, [FromBody] AppUserViewModel userViewModel)
        {
            //Bước 1: Khởi tạo các giá trị mặc định
            var user = await _userManager.FindByIdAsync(userViewModel.Id.ToString()) ?? new AppUser();
            //Bước 2: Validate dữ liệu đầu vào (Lấy từ ViewModel ra)
            var msgs = UserValidate.Validate(userViewModel);
            if (msgs.Count > 0) return BadRequest(string.Join("</br>", msgs));
            //Bước 3: Thực hiện nghiệp vụ
            user.DateModified = DateTime.Now;
            user.UserIdModified = User.GetUserId();
            user.Type = userViewModel.Type;
            userViewModel = _unitOfWork.UserRepository.UpdateAsync(user, userViewModel ?? new AppUserViewModel());
            if (userViewModel == new AppUserViewModel()) return NotFound();
            if (userViewModel?.Type != UserType.Admin)
                await _unitOfWork.UserRepository.UpdateRoles(user ?? new AppUser(), userViewModel);
            //Bước 4: Ghi lịch sử
            await _unitOfWork.LogHistoryRepository.InsertAsync(new LogHistory()
            {
                UserId = User.GetUserId(),
                ObjectId = userViewModel!.Id,
                Url = Request.Path.Value ?? "/",
                Action = ConstSystem.Update,
                Content = "Cập nhật đối tượng",
            });
            await _unitOfWork.SaveAsync();
            return Ok();
        }

        [HttpPut("ChangePassWord")]
        [Authorize]
        public async Task<IActionResult> ChangePassWord([FromBody] ChangePasswordViewModel changePasswordViewModel)
        {
            var user = await _userManager.FindByIdAsync(User.GetUserId().ToString());
            if (user == null) return NotFound();
            var result = await _userManager.ChangePasswordAsync(user, changePasswordViewModel?.PasswordOld ?? "", changePasswordViewModel?.PasswordNew ?? "");
            if (!result.Succeeded) return BadRequest(string.Join("<br>", result.Errors.Select(x => x.Description)));
            return Ok();
        }

        [HttpGet("{id}")]
        [Authorize(Permissions.User.View)]
        public async Task<ActionResult<AppUserViewModel>> FindByIdAsync(Guid id)
        {
            var userViewModel = await _unitOfWork.UserRepository.FindByIdAsync(id);
            return Ok(userViewModel);
        }

        [HttpDelete("RemoveAsync/{id}")]
        [Authorize(Permissions.User.Remove)]
        public async Task<IActionResult> RemoveAsync(Guid id)
        {
            await _unitOfWork.UserRepository.RemoveAsync(id);
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
        [Authorize(Permissions.User.View)]
        public async Task<IActionResult> Select(string? q = "")
        {
            var userViewModels = await _unitOfWork.UserRepository.SelectAsync(q);
            return Ok(userViewModels);
        }
    }
}