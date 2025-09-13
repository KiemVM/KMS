using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using KMS.Common.Constants;
using KMS.Common.Helper;
using KMS.Core.Entities.Identity;
using KMS.Core.ViewModels.Identity;
using KMS.Data.SeedWorks;
using KMS.Data.Services;
using KMS.WebApp.Extensions;
using System.Data;
using System.Security.Claims;
using System.Text.Json;
using static KMS.Common.Constants.Permissions;

namespace KMS.WebApp.Controllers.Identity
{
    public class AuthController : ApiControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IDistributedCache _distributedCache;

        public AuthController(UserManager<AppUser> userManager, IUnitOfWork unitOfWork, SignInManager<AppUser> signInManager, ITokenService tokenService, RoleManager<AppRole> roleManager, IDistributedCache distributedCache)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _roleManager = roleManager;
            _unitOfWork = unitOfWork;
            _distributedCache = distributedCache;
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<ActionResult<AuthenticatedResult>> Login([FromBody] LoginViewModel? request)
        {
            try
            {
                //Authentication
                if (request is null || string.IsNullOrWhiteSpace(request.UserName) || string.IsNullOrWhiteSpace(request.Password)) return BadRequest("Bạn chưa nhập thông tin đăng nhập");

                var user = await _userManager.FindByNameAsync(request.UserName ?? "");
                if (user is null) return BadRequest("Tên đăng nhập không tồn tại");
                if (!user.IsActive || user.LockoutEnabled) return BadRequest("Tài khoản của bạn bị khóa, vui lòng liên hệ Admin!");

                var result = await _signInManager.PasswordSignInAsync(request.UserName ?? "", request.Password ?? "", false, true);
                if (!result.Succeeded) return BadRequest("Mật khẩu không đúng, vui lòng thử lại");


                //Authorization
                var roles = await _userManager.GetRolesAsync(user);
                var permissions = await this.GetPermissionsByUserIdAsync(user);
                var objectName = string.Empty;


                var claims = new List<Claim>()
            {
                new Claim(UserClaims.Email, user?.Email ?? ""),
                new Claim(UserClaims.UserName, user?.UserName ?? ""),
                new Claim(UserClaims.Id, user?.Id.ToString()?? ""),
                new Claim(UserClaims.FullName, user?.FullName ?? ""),
                new Claim(UserClaims.Avatar, user?.Avatar ?? ""),
                new Claim(UserClaims.Roles,JsonSerializer.Serialize(roles)),
                new Claim(UserClaims.Permissions, JsonSerializer.Serialize(permissions))
            };

                var accessToken = _tokenService.GenerateAccessToken(claims);
                var refreshToken = _tokenService.GenerateRefreshToken();

                user.RefreshToken = refreshToken;
                user.RefreshTokenExpiryTime = DateTime.Now.AddDays(30);
                await _userManager.UpdateAsync(user);

                var cachedEmpsPermissionsJson = await _distributedCache.GetStringAsync($"Pms_{user.Id}");
                var cachedEmpsRole = await _distributedCache.GetStringAsync($"Role_{user.Id}");
                var tokenAuthorize = await _distributedCache.GetStringAsync($"Token_{user.Id}");
                await _distributedCache.SetStringAsync(
                    $"Pms_{user.Id}",
                    JsonSerializer.Serialize(permissions),
                    new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1) // thời hạn là 30 phút
                    });
                await _distributedCache.SetStringAsync($"Role_{user.Id}", string.Join(";", roles), new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1) // thời hạn là 30 phút
                });
                await _distributedCache.SetStringAsync($"Token_{user.Id}", accessToken, new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1) // thời hạn là 30 phút
                });

                Response.Cookies.Append("Authorization", $"Token_{user.Id}", new CookieOptions
                {
                    Expires = DateTime.Now.AddDays(1),
                    HttpOnly = true
                });
                Response.Cookies.Append("access-token", accessToken, new CookieOptions
                {
                    Expires = DateTime.Now.AddDays(1),
                    HttpOnly = true
                });

                Response.Cookies.Append("refresh-token", refreshToken, new CookieOptions
                {
                    Expires = DateTime.Now.AddDays(1),
                    HttpOnly = true
                });

                return Ok(new AuthenticatedResult()
                {
                    Token = accessToken,
                    Roles = JsonSerializer.Serialize(roles),
                    Permissions = JsonSerializer.Serialize(permissions),
                    RefreshToken = refreshToken
                });
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        private async Task<List<string>> GetPermissionsByUserIdAsync(AppUser user)
        {
            var roles = await _userManager.GetRolesAsync(user!);
            var permissions = new List<string>();
            if (!roles.Contains(ConstSystem.RoleAdmin))
            {
                foreach (var roleName in roles)
                {
                    var role = await _roleManager.FindByNameAsync(roleName);
                    if (role is null) continue;
                    var claims = await _roleManager.GetClaimsAsync(role);
                    var roleClaimValues = claims.Select(x => x.Value).ToList();
                    permissions.AddRange(roleClaimValues);
                }
            }
            return permissions.Distinct().ToList();
        }

        [HttpGet("Logout")]
        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            await _distributedCache.RemoveAsync($"Pms_{User.Claims.FirstOrDefault(x => x.Value == "Id")}");
            await _distributedCache.RemoveAsync($"Role_{User.Claims.FirstOrDefault(x => x.Value == "Id")}");
            await _distributedCache.RemoveAsync($"Token_{User.Claims.FirstOrDefault(x => x.Value == "Id")}");
            await _signInManager.SignOutAsync();
            Response.Cookies.Append("access-token", "", new CookieOptions
            {
                Expires = DateTime.Now.AddDays(-1)
            });

            Response.Cookies.Append("refresh-token", "", new CookieOptions
            {
                Expires = DateTime.Now.AddDays(-1)
            });
            Response.Cookies.Append("Authorization", "", new CookieOptions
            {
                Expires = DateTime.Now.AddDays(-1)
            });
            return Ok();
        }
    }
}