using AutoMapper;
using Microsoft.AspNetCore.Identity;
using KMS.Core.Entities.Identity;
using KMS.Data.Repositories.Identity;
using KMS.Data.Repositories.Log;
using KMS.Data.Repositories.Content;

namespace KMS.Data.SeedWorks
{
    public interface IUnitOfWork : IDisposable
    {
        KMSContext Context { get; }
        ILogHistoryRepository LogHistoryRepository { get; }
        IUserRepository UserRepository { get; }
        IRoleRepository RoleRepository { get; }
        ICompanyRepository CompanyRepository { get; }
        IApplicationRepository ApplicationRepository { get; }
        IKeyRepository KeyRepository { get; }
        Task SaveAsync();
    }

    public class UnitOfWork : IUnitOfWork
    {
        private readonly KMSContext _context;
        private readonly IMapper _mapper;

        public UnitOfWork(KMSContext context, IMapper mapper, UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            _context = context;
            _mapper = mapper;
            LogHistoryRepository = new LogHistoryRepository(context);
            UserRepository = new UserRepository(context, mapper, userManager);
            RoleRepository = new RoleRepository(context, mapper, roleManager);
            CompanyRepository = new CompanyRepository(context, _mapper);
            ApplicationRepository = new ApplicationRepository(context, _mapper);
            KeyRepository = new KeyRepository(context, _mapper);
        }

        public KMSContext Context => _context;
        public ILogHistoryRepository LogHistoryRepository { get; private set; }
        public IUserRepository UserRepository { get; private set; }
        public IRoleRepository RoleRepository { get; private set; }

        public ICompanyRepository CompanyRepository { get; private set; }
        public IApplicationRepository ApplicationRepository { get; private set; }
        public IKeyRepository KeyRepository { get; private set; }

        public async Task SaveAsync()
        {
            // In tại đây trước khi SaveAsync
            var debug = _context.ChangeTracker.DebugView.LongView;
            Console.WriteLine(debug);
            await _context.SaveChangesAsync();
        }

        private bool _disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this._disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}