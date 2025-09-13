using AutoMapper;
using Microsoft.EntityFrameworkCore;
using KMS.Common.Helper;
using KMS.Core.Entities.Content;
using KMS.Core.ViewModels;
using KMS.Core.ViewModels.Content;
using KMS.Core.ViewModels.Extension;
using KMS.Core.ViewModels.Identity;
using KMS.Data.SeedWorks;
using CompanyViewModel = KMS.Core.ViewModels.Content.CompanyViewModel;

namespace KMS.Data.Repositories.Content
{
    public interface ICompanyRepository : IRepositoryBase<Company>
    {
        Task<PagedResult<KMS.Core.ViewModels.Content.CompanyViewModel>> PagingAsync(int page, int pageSize, string search, CompanyStatus companyStatus);

        Task<KMS.Core.ViewModels.Content.CompanyViewModel> InsertAsync(KMS.Core.ViewModels.Content.CompanyViewModel companyViewModel);

        Task<KMS.Core.ViewModels.Content.CompanyViewModel> UpdateAsync(KMS.Core.ViewModels.Content.CompanyViewModel companyViewModel);

        Task<KMS.Core.ViewModels.Content.CompanyViewModel> FindByIdAsync(Guid id);

        Task<List<ComboboxViewModel>> SelectAsync(string? q);

        Task RemoveAsync(Guid id);
        Task<List<ComboboxTreeViewModel>> SelectTreeByTypeAsync(string? q = "");
        Task<List<CompanyViewModel>> GetAllCompany();
    }

    public class CompanyRepository : RepositoryBase<Company>, ICompanyRepository
    {
        private readonly IMapper _mapper;

        public CompanyRepository(KMSContext context, IMapper mapper) : base(context)
        {
            _mapper = mapper;
        }

        public async Task<List<ComboboxTreeViewModel>> SelectTreeByTypeAsync(string? q = "")
        {
            var allCategories = await (from company in FindAll()
                where company.Status != CompanyStatus.Delete
                select new ComboboxTreeViewModel()
                {
                    Value = company.Id,
                    Name = company.Name,
                    Description = company.Description,
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

        public async Task<List<CompanyViewModel>> GetAllCompany()
        {
            var query = from company in FindAll()
                join user in _context.Users on company.UserIdCreated equals user.Id
                where company.Status != CompanyStatus.Delete
                orderby company.Count
                select new KMS.Core.ViewModels.Content.CompanyViewModel()
                {
                    Id = company.Id,
                    DateCreated = company.DateCreated,
                    DateModified = company.DateModified,
                    UserIdCreated = company.UserIdCreated,
                    UserIdModified = company.UserIdModified,
                    Number = company.Number,
                    Status = company.Status,
                    AvatarViewModel = AvatarViewModel.UpdateAvatar(user),
                    Name = company.Name,
                    Address = company.Address,
                    Phone = company.Phone,
                    Email = company.Phone
                };
            return query.ToList();
        }

        public async Task<PagedResult<KMS.Core.ViewModels.Content.CompanyViewModel>> PagingAsync(int page, int pageSize, string search, CompanyStatus companyStatus)
        {
            var query = from company in FindAll()
                        join user in _context.Users on company.UserIdCreated equals user.Id
                        orderby company.Count
                        select new KMS.Core.ViewModels.Content.CompanyViewModel()
                        {
                            Id = company.Id,
                            DateCreated = company.DateCreated,
                            DateModified = company.DateModified,
                            UserIdCreated = company.UserIdCreated,
                            UserIdModified = company.UserIdModified,
                            Number = company.Number,
                            Status = company.Status,
                            AvatarViewModel = AvatarViewModel.UpdateAvatar(user),
                            Name = company.Name,
                            Address = company.Address,
                            Phone = company.Phone,
                            Email = company.Phone
                        };
            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();
                query = query.Where(x => (x.Name ?? "").Contains(search));
            }
            if (companyStatus != CompanyStatus.All)
            {
                query = query.Where(x => x.Status == companyStatus);
            }
            return new PagedResult<KMS.Core.ViewModels.Content.CompanyViewModel>
            {
                Results = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(),
                CurrentPage = page,
                RowCount = await query.CountAsync(),
                PageSize = pageSize
            };
        }

        public async Task<KMS.Core.ViewModels.Content.CompanyViewModel> InsertAsync(KMS.Core.ViewModels.Content.CompanyViewModel companyViewModel)
        {
            Company? company = new Company() { Id = Guid.NewGuid() };
            company.UpdateViewModel(companyViewModel);
            company.UserIdCreated = companyViewModel.UserIdCreated;
            company.Count = ((await FindAll().OrderBy(x => x.Count).LastOrDefaultAsync())?.Count ?? 0) + 1;
            company.Number = CompanyConst.PreNumber + company.Count.ToString("0000");
            await AddAsync(company);
            companyViewModel.Id = company.Id;
            companyViewModel.Number = company.Number;
            return companyViewModel;
        }

        public async Task<KMS.Core.ViewModels.Content.CompanyViewModel> UpdateAsync(KMS.Core.ViewModels.Content.CompanyViewModel companyViewModel)
        {
            Company? company = await FindIdAsync(companyViewModel.Id);
            if (company is null) throw new ServiceExceptionHelper("Không tồn tại đối tượng này.");
            company.UpdateViewModel(companyViewModel);
            company.UserIdModified = companyViewModel.UserIdModified;
            Update(company);
            return companyViewModel;
        }

        public async Task<KMS.Core.ViewModels.Content.CompanyViewModel> FindByIdAsync(Guid id)
        {
            var company = await _context.Companies
                .Where(c => c.Id == id && c.Status != CompanyStatus.Delete)
                .Select(c => new KMS.Core.ViewModels.Content.CompanyViewModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    Slug = c.Slug,
                    Code = c.Code,
                    Description = c.Description,
                    Image = c.Image,
                    Address = c.Address,
                    Phone = c.Phone,
                    Email = c.Email,
                    Status = c.Status,
                    Applications = _context.Applications
                        .Where(a => a.CompanyId == c.Id && a.Status != ApplicationStatus.Delete)
                        .Select(a => new ApplicationViewModel
                        {
                            Id = a.Id,
                            Name = a.Name,
                            Code = a.Code,
                            Description = a.Description,
                            Image = a.Image,
                            CompanyId = a.CompanyId,
                            Status = a.Status,
                            Keys = _context.Keys
                                .Where(s => s.ApplicationId == a.Id)
                                .Select(s => new KeyViewModel
                                {
                                    Id = s.Id,
                                    Name = s.Name,
                                    Description = s.Description,
                                    ApplicationId = s.ApplicationId,
                                    KeyData = s.KeyData,
                                    Status = s.Status,
                                }).ToList()
                        }).ToList(),
                    Keys = _context.Keys
                        .Where(s => _context.Applications
                            .Where(a => a.CompanyId == c.Id)
                            .Select(a => a.Id)
                            .Contains(s.ApplicationId))
                        .Select(s => new KeyViewModel
                        {
                            Id = s.Id,
                            Name = s.Name,
                            Description = s.Description,
                            ApplicationId = s.ApplicationId,
                            KeyData = s.KeyData,
                            Status = s.Status,
                        }).ToList()
                })
                .FirstOrDefaultAsync();

            return company ?? new KMS.Core.ViewModels.Content.CompanyViewModel();
        }

        public async Task<List<ComboboxViewModel>> SelectAsync(string? q)
        {
            var query = from company in FindAll()
                        where company.Status != CompanyStatus.Delete
                        select new ComboboxViewModel()
                        {
                            Value = company.Id,
                            Name = company.Number,
                            Description = company.Name,
                        };
            if (!string.IsNullOrWhiteSpace(q))
            {
                query = query.Where(x => (x.Name ?? "").Contains(q));
            }

            return await query.ToListAsync();
        }

        public async Task RemoveAsync(Guid id)
        {
            Company? company = await FindIdAsync(id);
            if (company is null) throw new ServiceExceptionHelper("Không tồn tại đối tượng này.");
            company.Status = CompanyStatus.Delete;
            Update(company);
        }
    }
}
