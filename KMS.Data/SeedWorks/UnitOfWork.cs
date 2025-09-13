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
        SaaSContext Context { get; }
        ILogHistoryRepository LogHistoryRepository { get; }
        IUserRepository UserRepository { get; }
        IRoleRepository RoleRepository { get; }
        ICompanyRepository CompanyRepository { get; }
        IApplicationRepository ApplicationRepository { get; }
        ISchemaRepository SchemaRepository { get; }
        ITableRepository TableRepository { get; }
        ITableDetailRepository TableDetailRepository { get; }
        Task SaveAsync();
    }

    public class UnitOfWork : IUnitOfWork
    {
        private readonly SaaSContext _context;
        private readonly IMapper _mapper;

        public UnitOfWork(SaaSContext context, IMapper mapper, UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            _context = context;
            _mapper = mapper;
            LogHistoryRepository = new LogHistoryRepository(context);
            UserRepository = new UserRepository(context, mapper, userManager);
            RoleRepository = new RoleRepository(context, mapper, roleManager);
            CompanyRepository = new CompanyRepository(context, _mapper);
            ApplicationRepository = new ApplicationRepository(context, _mapper);
            SchemaRepository = new SchemaRepository(context, _mapper);
            TableDetailRepository = new TableDetailRepository(context, _mapper);
            TableRepository = new TableRepository(context, _mapper);
        }

        public SaaSContext Context => _context;
        public ILogHistoryRepository LogHistoryRepository { get; private set; }
        public IUserRepository UserRepository { get; private set; }
        public IRoleRepository RoleRepository { get; private set; }

        public ICompanyRepository CompanyRepository { get; private set; }
        public IApplicationRepository ApplicationRepository { get; private set; }
        public ISchemaRepository SchemaRepository { get; private set; }
        public ITableRepository TableRepository { get; private set; }
        public ITableDetailRepository TableDetailRepository { get; private set; }

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