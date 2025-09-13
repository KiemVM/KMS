using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using KMS.Common.Helper;
using KMS.Core.Entities.Identity;
using KMS.Core.ViewModels;
using KMS.Core.ViewModels.Extension;
using KMS.Core.ViewModels.Identity;
using KMS.Data.SeedWorks;
using Azure;
using static KMS.Common.Constants.Permissions;

namespace KMS.Data.Repositories.Identity
{
    public interface IUserRepository : IRepositoryBase<AppUser>
    {
        Task<PagedResult<AppUserViewModel>> PagingAsync(int page, int pageSize, string search, Guid userId, UserType userType);

        Task<List<AppUserViewModel>> GetAllAsync();

        Task<AppUserViewModel> InsertAsync(AppUserViewModel appUserViewModel);

        AppUserViewModel UpdateAsync(AppUser? appUser, AppUserViewModel appUserViewModel);

        Task<AppUserViewModel> FindByIdAsync(Guid id);

        Task<IEnumerable<ComboboxViewModel>> SelectAsync(string? q);

        Task RemoveAsync(Guid id);

        Task RemoveUserFromRoles(Guid id, string[] roleNames);

        Task UpdateRoles(AppUser user, AppUserViewModel? userViewModel);
    }

    public class UserRepository : RepositoryBase<AppUser>, IUserRepository
    {
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;

        public UserRepository(SaaSContext context, IMapper mapper, UserManager<AppUser> userManager) : base(context)
        {
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<PagedResult<AppUserViewModel>> PagingAsync(int page, int pageSize, string search, Guid userId, UserType userType)
        {
            var query = from appUser in FindAll()

                        join userCreate in FindAll() on appUser.UserIdCreated equals userCreate.Id into userCreates
                        from userCreate in userCreates.DefaultIfEmpty()

                        where appUser.Status != UserStatus.Delete
                        orderby appUser.Sort
                        select new AppUserViewModel()
                        {
                            Id = appUser.Id,
                            Type = appUser.Type,
                            Position = appUser.Position,
                            Dob = appUser.Dob,
                            Avatar = appUser.Avatar,
                            PhoneNumber = appUser.PhoneNumber,
                            BankNumber = appUser.BankNumber,
                            UserName = appUser.UserName,
                            FullName = appUser.FullName,
                            Email = appUser.Email,
                            Status = appUser.Status,
                            AvatarViewModel = AvatarViewModel.UpdateAvatar(userCreate)
                        };
            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();
                query = query.Where(x => (x.UserName ?? "").Contains(search)
                                        || (x.Email ?? "").Contains(search)
                                         || (x.FullName ?? "").Contains(search));
            }
            if (userType != UserType.All)
            {
                query = query.Where(x => x.Type == userType);
            }
            if (userId != Guid.Empty)
            {
                query = query.Where(x => x.Id == userId);
            }
            return new PagedResult<AppUserViewModel>
            {
                Results = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(),
                CurrentPage = page,
                RowCount = await query.CountAsync(),
                PageSize = pageSize
            };
        }

        public async Task<List<AppUserViewModel>> GetAllAsync()
        {
            var users = await (
                from appUser in FindAll()

                join userCreate in FindAll() on appUser.UserIdCreated equals userCreate.Id into userCreates
                from userCreate in userCreates.DefaultIfEmpty()

                where appUser.Status != UserStatus.Delete
                orderby appUser.Sort
                select new AppUserViewModel()
                {
                    Id = appUser.Id,
                    Type = appUser.Type,
                    Position = appUser.Position,
                    Dob = appUser.Dob,
                    Avatar = appUser.Avatar,
                    PhoneNumber = appUser.PhoneNumber,
                    BankNumber = appUser.BankNumber,
                    UserName = appUser.UserName,
                    FullName = appUser.FullName,
                    Email = appUser.Email,
                    Status = appUser.Status,
                    AvatarViewModel = AvatarViewModel.UpdateAvatar(userCreate)
                }).ToListAsync();

            return users;
        }

        public async Task<AppUserViewModel> InsertAsync(AppUserViewModel appUserViewModel)
        {
            AppUser? appUser = new AppUser() { Id = Guid.NewGuid() };
            appUser.UpdateViewModel(appUserViewModel);
            await AddAsync(appUser);
            appUserViewModel.Id = appUser.Id;
            return appUserViewModel;
        }

        public AppUserViewModel UpdateAsync(AppUser? appUser, AppUserViewModel appUserViewModel)
        {
            if (appUser is null) throw new ServiceExceptionHelper("Không tồn tại đối tượng này.");
            appUser.UpdateViewModel(appUserViewModel);
            appUser.PasswordHash = _userManager.PasswordHasher.HashPassword(appUser, appUserViewModel?.PasswordSha256 ?? "");
            appUser.PasswordSha256 = appUserViewModel?.PasswordSha256.EncryptString();
            Update(appUser);
            return appUserViewModel!;
        }

        public async Task<AppUserViewModel> FindByIdAsync(Guid id)
        {
            AppUser? appUser = await FindIdAsync(id);
            if (appUser is null) return new AppUserViewModel();
            return _mapper.Map<AppUserViewModel>(appUser);
        }

        public async Task<IEnumerable<ComboboxViewModel>> SelectAsync(string? q)
        {
            var query = from appUser in FindAll()
                        select new ComboboxViewModel()
                        {
                            Name = appUser.UserName,
                            Value = appUser.Id,
                            Description = appUser.FullName,
                            Avatar = appUser.Avatar,
                        };
            if (!string.IsNullOrWhiteSpace(q))
            {
                query = query.Where(x => (x.Name ?? "").Contains(q));
            }

            return await query.ToListAsync();
        }

        public async Task RemoveAsync(Guid id)
        {
            AppUser? appUser = await FindIdAsync(id);
            if (appUser is null) throw new ServiceExceptionHelper("Không tồn tại đối tượng này.");
            appUser.Status = UserStatus.Delete;
            await RemoveAsync(id);
        }

        public async Task RemoveUserFromRoles(Guid id, string[]? roleNames)
        {
            if (roleNames == null || roleNames.Length == 0)
                return;
            foreach (var roleName in roleNames)
            {
                var role = await _context.Roles.FirstOrDefaultAsync(x => x.Name == roleName);
                if (role == null)
                {
                    return;
                }
                var userRole = await _context.UserRoles.FirstOrDefaultAsync(x => x.RoleId == role.Id && x.UserId == id);
                if (userRole == null)
                {
                    return;
                }
                _context.UserRoles.Remove(userRole);
            }
        }

        public async Task UpdateRoles(AppUser user, AppUserViewModel? userViewModel)
        {
            var currentRoles = await _userManager.GetRolesAsync(user);
            await RemoveUserFromRoles(user.Id, currentRoles.ToArray());
            //Thực hiện phân quyền tại đây
            if (userViewModel?.Type == UserType.User)
            {
                await _userManager.AddToRoleAsync(user, "User");
            }
        }
    }
}