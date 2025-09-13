using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using KMS.Common.Constants;
using KMS.Common.Helper;
using KMS.Core.Entities.Identity;
using KMS.Core.ViewModels;
using KMS.Core.ViewModels.Extension;
using KMS.Core.ViewModels.Identity;
using KMS.Data.SeedWorks;

namespace KMS.Data.Repositories.Identity
{
    public interface IRoleRepository : IRepositoryBase<AppRole>
    {
        Task<PagedResult<AppRoleViewModel>> PagingAsync(int page, int pageSize, string search);

        Task<AppRoleViewModel> InsertAsync(AppRoleViewModel appRoleViewModel);

        AppRoleViewModel UpdateAsync(AppRole? appRole, AppRoleViewModel appRoleViewModel);

        Task<AppRoleViewModel> FindByIdAsync(Guid id);

        Task<IEnumerable<ComboboxViewModel>> SelectAsync(string? q);

        Task RemoveAsync(Guid id);

        Task<List<PermissionViewModel>> GetPermissionsByRole(Guid id);

        Task<List<AppRoleViewModel>> GetRoleByUser(Guid userId);
    }

    public class RoleRepository : RepositoryBase<AppRole>, IRoleRepository
    {
        private readonly IMapper _mapper;
        private readonly RoleManager<AppRole> _roleManager;

        public RoleRepository(KMSContext context, IMapper mapper, RoleManager<AppRole> roleManager) : base(context)
        {
            _mapper = mapper;
            _roleManager = roleManager;
        }

        public async Task<PagedResult<AppRoleViewModel>> PagingAsync(int page, int pageSize, string search)
        {
            var query = from appRole in FindAll()
                        where appRole.Name != ConstSystem.RoleAdmin
                        select new AppRoleViewModel()
                        {
                            Id = appRole.Id,
                            Name = appRole.Name,
                            DisplayName = appRole.DisplayName,
                        };
            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();
                query = query.Where(x => (x.Name ?? "").Contains(search)
                                         || (x.DisplayName ?? "").Contains(search));
            }
            return new PagedResult<AppRoleViewModel>
            {
                Results = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(),
                CurrentPage = page,
                RowCount = await query.CountAsync(),
                PageSize = pageSize
            };
        }

        public async Task<AppRoleViewModel> InsertAsync(AppRoleViewModel appRoleViewModel)
        {
            AppRole? appRole = new AppRole() { Id = Guid.NewGuid() };
            appRole.UpdateViewModel(appRoleViewModel);
            await AddAsync(appRole);
            appRoleViewModel.Id = appRole.Id;
            return appRoleViewModel;
        }

        public AppRoleViewModel UpdateAsync(AppRole? appRole, AppRoleViewModel appRoleViewModel)
        {
            if (appRole is null) throw new ServiceExceptionHelper("Không tồn tại đối tượng này.");
            appRole.UpdateViewModel(appRoleViewModel);
            Update(appRole);
            return appRoleViewModel;
        }

        public async Task<AppRoleViewModel> FindByIdAsync(Guid id)
        {
            AppRole? appRole = await FindIdAsync(id);
            if (appRole is null) return new AppRoleViewModel();
            return _mapper.Map<AppRoleViewModel>(appRole);
        }

        public async Task<IEnumerable<ComboboxViewModel>> SelectAsync(string? q)
        {
            var query = from appRole in FindAll()
                        select new ComboboxViewModel()
                        {
                            Name = appRole.Name,
                            Value = appRole.Id,
                            Description = appRole.DisplayName,
                        };
            if (!string.IsNullOrWhiteSpace(q))
            {
                query = query.Where(x => (x.Name ?? "").Contains(q));
            }

            return await query.ToListAsync();
        }

        public async Task RemoveAsync(Guid id)
        {
            AppRole? appRole = await FindIdAsync(id);
            if (appRole is null) throw new ServiceExceptionHelper("Không tồn tại đối tượng này.");
            Remove(appRole);
        }

        public async Task<List<PermissionViewModel>> GetPermissionsByRole(Guid id)
        {
            List<PermissionViewModel> permissionViewModels = ClaimExtensions.GetPermissions();
            if (id == Guid.Empty) return permissionViewModels;
            AppRole? appRole = await FindIdAsync(id);
            if (appRole is null) return permissionViewModels;

            var claims = await _roleManager.GetClaimsAsync(appRole);
            var roleClaimValues = claims.Select(a => a.Value).ToList();

            foreach (var permissionViewModel in permissionViewModels)
            {
                var allClaimValues = permissionViewModel.RoleClaimsViewModels?.Select(a => a.Value).ToList();
                var authorizedClaims = allClaimValues?.Intersect(roleClaimValues).ToList();
                foreach (var roleClaimsViewModel in permissionViewModel.RoleClaimsViewModels ?? new List<RoleClaimsViewModel>())
                {
                    if (authorizedClaims?.Any(a => a == roleClaimsViewModel?.Value) ?? false)
                        roleClaimsViewModel.Selected = true;
                }
            }
            return permissionViewModels;
        }

        public async Task<List<AppRoleViewModel>> GetRoleByUser(Guid userId)
        {
            if (userId == Guid.Empty) return new List<AppRoleViewModel>();
            var roles = from appRole in FindAll()
                        join userRole in _context.UserRoles on appRole.Id equals userRole.RoleId
                        where userRole.UserId == userId
                        select new AppRoleViewModel()
                        {
                            Name = appRole.Name,
                            DisplayName = appRole.DisplayName,
                        };
            return await roles.ToListAsync();
        }
    }
}