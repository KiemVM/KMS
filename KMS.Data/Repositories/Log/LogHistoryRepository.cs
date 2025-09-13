using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using KMS.Core.Entities.Identity;
using KMS.Core.Entities.Log;
using KMS.Core.ViewModels;
using KMS.Core.ViewModels.Identity;
using KMS.Core.ViewModels.Log;
using KMS.Data.SeedWorks;

namespace KMS.Data.Repositories.Log
{
    public interface ILogHistoryRepository : IRepositoryBase<LogHistory>
    {
        Task<PagedResult<LogHistoryViewModel>> PagingAsync(int page, int pageSize, string search);

        Task InsertAsync(LogHistory logHistory);
    }

    public class LogHistoryRepository : RepositoryBase<LogHistory>, ILogHistoryRepository
    {
        public LogHistoryRepository(KMSContext context) : base(context)
        {
        }

        public async Task<PagedResult<LogHistoryViewModel>> PagingAsync(int page, int pageSize, string search)
        {
            var query = from logHistory in _context.LogHistories
                        join user in _context.Users on logHistory.UserId equals user.Id
                        select new LogHistoryViewModel
                        {
                            Id = logHistory.Id,
                            Content = logHistory.Content ?? "",
                            UserId = logHistory.UserId,
                            DateCreated = logHistory.DateCreated,
                            DateModified = logHistory.DateModified,
                            UserViewModel = AvatarViewModel.UpdateAvatar(user)
                        };
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(x => (x.Content ?? "").Contains(search));
            }
            query = query.OrderByDescending(x => x.DateModified);
            return new PagedResult<LogHistoryViewModel>
            {
                Results = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(),
                CurrentPage = page,
                RowCount = await query.CountAsync(),
                PageSize = pageSize,
            };
        }

        public async Task InsertAsync(LogHistory logHistory)
        {

            await AddAsync(logHistory);
        }
    }
}