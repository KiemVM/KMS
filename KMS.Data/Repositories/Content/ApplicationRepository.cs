using AutoMapper;
using Microsoft.EntityFrameworkCore;
using KMS.Common.Helper;
using KMS.Core.Entities.Content;
using KMS.Core.ViewModels;
using KMS.Core.ViewModels.Content;
using KMS.Core.ViewModels.Extension;
using KMS.Core.ViewModels.Identity;
using KMS.Data.SeedWorks;

namespace KMS.Data.Repositories.Content
{
    public interface IApplicationRepository : IRepositoryBase<Application>
    {
        Task<PagedResult<ApplicationViewModel>> PagingAsync(int page, int pageSize, string search, ApplicationStatus applicationStatus, Guid companyId, Guid databaseId);

        Task<ApplicationViewModel> InsertAsync(ApplicationViewModel applicationViewModel);

        Task<ApplicationViewModel> UpdateAsync(ApplicationViewModel applicationViewModel);

        Task<ApplicationViewModel> FindByIdAsync(Guid id);

        Task<List<ComboboxViewModel>> SelectAsync(string? q);

        Task RemoveAsync(Guid id);
        Task<List<ComboboxTreeViewModel>> SelectTreeByTypeAsync(string? q = "");
    }

    public class ApplicationRepository : RepositoryBase<Application>, IApplicationRepository
    {
        private readonly IMapper _mapper;

        public ApplicationRepository(SaaSContext context, IMapper mapper) : base(context)
        {
            _mapper = mapper;
        }

        public async Task<List<ComboboxTreeViewModel>> SelectTreeByTypeAsync(string? q = "")
        {
            var allCategories = await (from application in FindAll()
                where application.Status != ApplicationStatus.Delete
                select new ComboboxTreeViewModel()
                {
                    Value = application.Id,
                    Name = application.Name,
                    Description = application.Description,
                }).ToListAsync();

            if (!string.IsNullOrWhiteSpace(q))
            {
                allCategories = allCategories.Where(x => (x.Name ?? "").Contains(q)).ToList();
            }

            // Xây dựng cây và tính level
            var result = new List<ComboboxTreeViewModel>();
            BuildTree(allCategories, result, Guid.Empty, 0);

            return result;
        }

        private void BuildTree(List<ComboboxTreeViewModel> allCategories, List<ComboboxTreeViewModel> result, Guid parentId, int level)
        {
            var children = allCategories.Where(x => x.ParentId == parentId).OrderBy(x => x.Name);

            foreach (var child in children)
            {
                child.Level = level;
                // Tạo prefix để hiển thị level
                var prefix = new string('─', level * 2);
                if (level > 0)
                {
                    prefix = "├" + prefix;
                }
                child.DisplayName = prefix + " " + child.Name;

                result.Add(child);

                // Đệ quy để thêm các con của node hiện tại
                BuildTree(allCategories, result, child.Value, level + 1);
            }
        }

        public async Task<PagedResult<ApplicationViewModel>> PagingAsync(
    int page, int pageSize, string search, ApplicationStatus applicationStatus, Guid companyId, Guid schemaId)
        {
            var query = from application in FindAll()
                        join user in _context.Users on application.UserIdCreated equals user.Id
                        join company in _context.Companies on application.CompanyId equals company.Id
                        select new ApplicationViewModel()
                        {
                            Id = application.Id,
                            DateCreated = application.DateCreated,
                            DateModified = application.DateModified,
                            UserIdCreated = application.UserIdCreated,
                            UserIdModified = application.UserIdModified,
                            Number = application.Number,
                            Status = application.Status,
                            AvatarViewModel = AvatarViewModel.UpdateAvatar(user),
                            Name = application.Name,
                            CompanyName = company.Name,
                            CompanyId = application.CompanyId
                        };

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();
                query = query.Where(x => (x.Name ?? "").Contains(search)
                                         || (x.CompanyName ?? "").Contains(search));
            }
            if (companyId != Guid.Empty)
            {
                query = query.Where(x => x.CompanyId == companyId);
            }
            if (applicationStatus != ApplicationStatus.All)
            {
                query = query.Where(x => x.Status == applicationStatus);
            }
            // Filter theo SchemaId
            if (schemaId != Guid.Empty)
            {
                var appIds = await _context.Schemas
                    .Where(s => s.Id == schemaId)
                    .Select(s => s.ApplicationId)
                    .ToListAsync();

                query = query.Where(x => appIds.Contains(x.Id));
            }

            var resultList = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            // Gắn Schemas cho từng ApplicationViewModel
            var appIdsForSchemas = resultList.Select(x => x.Id).ToList();
            var schemas = await _context.Schemas
                .Where(s => appIdsForSchemas.Contains(s.ApplicationId))
                .ToListAsync();

            foreach (var appVm in resultList)
            {
                appVm.Schemas = schemas
                    .Where(s => s.ApplicationId == appVm.Id)
                    .Select(s => new SchemaViewModel
                    {
                        Id = s.Id,
                        Name = s.Name,
                        Description = s.Description,
                        Database = s.Database,
                        Status = s.Status
                    }).ToList();
            }

            return new PagedResult<ApplicationViewModel>
            {
                Results = resultList,
                CurrentPage = page,
                RowCount = await query.CountAsync(),
                PageSize = pageSize
            };
        }

        public async Task<ApplicationViewModel> InsertAsync(ApplicationViewModel applicationViewModel)
        {
            Application? application = new Application() { Id = Guid.NewGuid() };
            application.UpdateViewModel(applicationViewModel);
            application.UserIdCreated = applicationViewModel.UserIdCreated;
            application.Count = ((await FindAll().OrderBy(x => x.Count).LastOrDefaultAsync())?.Count ?? 0) + 1;
            application.Number = ApplicationConst.PreNumber + application.Count.ToString("0000");
            await AddAsync(application);
            applicationViewModel.Id = application.Id;
            applicationViewModel.Number = application.Number;
            return applicationViewModel;
        }

        public async Task<ApplicationViewModel> UpdateAsync(ApplicationViewModel applicationViewModel)
        {
            Application? application = await FindIdAsync(applicationViewModel.Id);
            if (application is null) throw new ServiceExceptionHelper("Không tồn tại đối tượng này.");
            application.UpdateViewModel(applicationViewModel);
            application.UserIdModified = applicationViewModel.UserIdModified;
            Update(application);
            return applicationViewModel;
        }

        public async Task<ApplicationViewModel> FindByIdAsync(Guid id)
        {
            // Lấy Application
            var application = await _context.Applications.FirstOrDefaultAsync(a => a.Id == id);
            if (application == null)
                return new ApplicationViewModel();

            // Lấy thông tin Company
            var company = await _context.Companies.FirstOrDefaultAsync(c => c.Id == application.CompanyId);

            // Lấy danh sách Schemas liên kết với Application
            var schemas = await _context.Schemas
                .Where(s => s.ApplicationId == application.Id)
                .ToListAsync();

            // Map sang ViewModel
            var appVm = new ApplicationViewModel
            {
                Id = application.Id,
                Name = application.Name,
                Code = application.Code,
                Description = application.Description,
                Image = application.Image,
                CompanyId = application.CompanyId,
                Status = application.Status,
                CompanyName = company?.Name ?? "",
                CompanyStatus = company?.Status ?? CompanyStatus.Create,
                Schemas = schemas.Select(s => new SchemaViewModel
                {
                    Id = s.Id,
                    Name = s.Name,
                    Description = s.Description,
                    Database = s.Database,
                    Status = s.Status,
                    ApplicationId = s.ApplicationId
                }).ToList()
            };

            return appVm;
        }

        public async Task<List<ComboboxViewModel>> SelectAsync(string? q)
        {
            var query = from application in FindAll()
                        where application.Status != ApplicationStatus.Delete
                        select new ComboboxViewModel()
                        {
                            Value = application.Id,
                            Name = application.Number,
                            Description = application.Name,
                        };
            if (!string.IsNullOrWhiteSpace(q))
            {
                query = query.Where(x => (x.Name ?? "").Contains(q));
            }

            return await query.ToListAsync();
        }

        public async Task RemoveAsync(Guid id)
        {
            Application? application = await FindIdAsync(id);
            if (application is null) throw new ServiceExceptionHelper("Không tồn tại đối tượng này.");
            application.Status = ApplicationStatus.Delete;
            Update(application);
        }
    }
}
