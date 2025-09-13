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
    public interface ITableDetailRepository : IRepositoryBase<TableDetail>
    {
        Task<PagedResult<TableDetailViewModel>> PagingAsync(int page, int pageSize, string search, TableDetailStatus tableDetailStatus);

        Task<TableDetailViewModel> InsertAsync(TableDetailViewModel tableDetailViewModel);

        Task<TableDetailViewModel> UpdateAsync(TableDetailViewModel tableDetailViewModel);

        Task<TableDetailViewModel> FindByIdAsync(Guid id);

        Task<List<ComboboxViewModel>> SelectAsync(string? q);

        Task RemoveAsync(Guid id);
    }

    public class TableDetailRepository : RepositoryBase<TableDetail>, ITableDetailRepository
    {
        private readonly IMapper _mapper;

        public TableDetailRepository(SaaSContext context, IMapper mapper) : base(context)
        {
            _mapper = mapper;
        }

        public async Task<PagedResult<TableDetailViewModel>> PagingAsync(int page, int pageSize, string search, TableDetailStatus tableDetailStatus)
        {
            var query = from tableDetail in FindAll()
                        join user in _context.Users on tableDetail.UserIdCreated equals user.Id
                        where tableDetail.Status != TableDetailStatus.Delete
                        select new TableDetailViewModel()
                        {
                            Id = tableDetail.Id,
                            DateCreated = tableDetail.DateCreated,
                            DateModified = tableDetail.DateModified,
                            UserIdCreated = tableDetail.UserIdCreated,
                            UserIdModified = tableDetail.UserIdModified,
                            Number = tableDetail.Number,
                            Status = tableDetail.Status,
                            AvatarViewModel = AvatarViewModel.UpdateAvatar(user)
                        };
            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();
                query = query.Where(x => (x.Number ?? "").Contains(search)
                                         || (x.AvatarViewModel!.UserName ?? "").Contains(search));
            }
            if (tableDetailStatus != TableDetailStatus.All)
            {
                query = query.Where(x => x.Status == tableDetailStatus);
            }
            return new PagedResult<TableDetailViewModel>
            {
                Results = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(),
                CurrentPage = page,
                RowCount = await query.CountAsync(),
                PageSize = pageSize
            };
        }

        public async Task<TableDetailViewModel> InsertAsync(TableDetailViewModel tableDetailViewModel)
        {
            TableDetail? tableDetail = new TableDetail() { Id = Guid.NewGuid() };
            tableDetail.UpdateViewModel(tableDetailViewModel);
            tableDetail.UserIdCreated = tableDetailViewModel.UserIdCreated;
            tableDetail.Count = ((await FindAll().OrderBy(x => x.Count).LastOrDefaultAsync())?.Count ?? 0) + 1;
            tableDetail.Number = TableDetailConst.PreNumber + tableDetail.Count.ToString("0000");
            await AddAsync(tableDetail);
            tableDetailViewModel.Id = tableDetail.Id;
            tableDetailViewModel.Number = tableDetail.Number;
            return tableDetailViewModel;
        }

        public async Task<TableDetailViewModel> UpdateAsync(TableDetailViewModel tableDetailViewModel)
        {
            TableDetail? tableDetail = await FindIdAsync(tableDetailViewModel.Id);
            if (tableDetail is null) throw new ServiceExceptionHelper("Không tồn tại đối tượng này.");
            tableDetail.UpdateViewModel(tableDetailViewModel);
            tableDetail.UserIdModified = tableDetailViewModel.UserIdModified;
            Update(tableDetail);
            return tableDetailViewModel;
        }

        public async Task<TableDetailViewModel> FindByIdAsync(Guid id)
        {
            TableDetail? tableDetail = await FindIdAsync(id);
            if (tableDetail is null) return new TableDetailViewModel();
            return _mapper.Map<TableDetailViewModel>(tableDetail);
        }

        public async Task<List<ComboboxViewModel>> SelectAsync(string? q)
        {
            var query = from tableDetail in FindAll()
                        where tableDetail.Status != TableDetailStatus.Delete
                        select new ComboboxViewModel()
                        {
                            Value = tableDetail.Id,
                            Name = tableDetail.Number,
                            Description = tableDetail.Name,
                        };
            if (!string.IsNullOrWhiteSpace(q))
            {
                query = query.Where(x => (x.Name ?? "").Contains(q));
            }

            return await query.ToListAsync();
        }

        public async Task RemoveAsync(Guid id)
        {
            TableDetail? tableDetail = await FindIdAsync(id);
            if (tableDetail is null) throw new ServiceExceptionHelper("Không tồn tại đối tượng này.");
            tableDetail.Status = TableDetailStatus.Delete;
            Update(tableDetail);
        }
    }
}
