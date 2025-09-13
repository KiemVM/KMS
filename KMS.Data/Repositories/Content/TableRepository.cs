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
    public interface ITableRepository : IRepositoryBase<Table>
    {
        Task<PagedResult<TableViewModel>> PagingAsync(int page, int pageSize, string search, TableStatus tableStatus);

        Task<TableViewModel> InsertAsync(TableViewModel tableViewModel);

        Task<TableViewModel> UpdateAsync(TableViewModel tableViewModel);

        Task<TableViewModel> FindByIdAsync(Guid id);

        Task<List<ComboboxViewModel>> SelectAsync(string? q);

        Task RemoveAsync(Guid id);
    }

    public class TableRepository : RepositoryBase<Table>, ITableRepository
    {
        private readonly IMapper _mapper;

        public TableRepository(SaaSContext context, IMapper mapper) : base(context)
        {
            _mapper = mapper;
        }

        public async Task<PagedResult<TableViewModel>> PagingAsync(int page, int pageSize, string search, TableStatus tableStatus)
        {
            var query = from table in FindAll()
                        join user in _context.Users on table.UserIdCreated equals user.Id
                        where table.Status != TableStatus.Delete
                        select new TableViewModel()
                        {
                            Id = table.Id,
                            DateCreated = table.DateCreated,
                            DateModified = table.DateModified,
                            UserIdCreated = table.UserIdCreated,
                            UserIdModified = table.UserIdModified,
                            Number = table.Number,
                            Status = table.Status,
                            AvatarViewModel = AvatarViewModel.UpdateAvatar(user)
                        };
            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();
                query = query.Where(x => (x.Number ?? "").Contains(search)
                                         || (x.AvatarViewModel!.UserName ?? "").Contains(search));
            }
            if (tableStatus != TableStatus.All)
            {
                query = query.Where(x => x.Status == tableStatus);
            }
            return new PagedResult<TableViewModel>
            {
                Results = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(),
                CurrentPage = page,
                RowCount = await query.CountAsync(),
                PageSize = pageSize
            };
        }

        public async Task<TableViewModel> InsertAsync(TableViewModel tableViewModel)
        {
            Table? table = new Table() { Id = Guid.NewGuid() };
            table.UpdateViewModel(tableViewModel);
            table.UserIdCreated = tableViewModel.UserIdCreated;
            table.Count = ((await FindAll().OrderBy(x => x.Count).LastOrDefaultAsync())?.Count ?? 0) + 1;
            table.Number = TableConst.PreNumber + table.Count.ToString("0000");
            await AddAsync(table);
            tableViewModel.Id = table.Id;
            tableViewModel.Number = table.Number;
            return tableViewModel;
        }

        public async Task<TableViewModel> UpdateAsync(TableViewModel tableViewModel)
        {
            Table? table = await FindIdAsync(tableViewModel.Id);
            if (table is null) throw new ServiceExceptionHelper("Không tồn tại đối tượng này.");
            table.UpdateViewModel(tableViewModel);
            table.UserIdModified = tableViewModel.UserIdModified;
            Update(table);
            return tableViewModel;
        }

        public async Task<TableViewModel> FindByIdAsync(Guid id)
        {
            Table? table = await FindIdAsync(id);
            if (table is null) return new TableViewModel();
            return _mapper.Map<TableViewModel>(table);
        }

        public async Task<List<ComboboxViewModel>> SelectAsync(string? q)
        {
            var query = from table in FindAll()
                        where table.Status != TableStatus.Delete
                        select new ComboboxViewModel()
                        {
                            Value = table.Id,
                            Name = table.Number,
                            Description = table.Name,
                        };
            if (!string.IsNullOrWhiteSpace(q))
            {
                query = query.Where(x => (x.Name ?? "").Contains(q));
            }

            return await query.ToListAsync();
        }

        public async Task RemoveAsync(Guid id)
        {
            Table? table = await FindIdAsync(id);
            if (table is null) throw new ServiceExceptionHelper("Không tồn tại đối tượng này.");
            table.Status = TableStatus.Delete;
            Update(table);
        }
    }
}
