using KMS.Core.AbstractClass;
using KMS.Core.Entities.Cache;
using KMS.Core.Entities.Content;
using KMS.Core.Entities.Identity;
using KMS.Core.Entities.Log;
using KMS.Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using KMS.Common.Constants;

namespace KMS.Data
{
    public class SaaSContext : IdentityDbContext<AppUser, AppRole, Guid>
    {
        public SaaSContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<LogHistory> LogHistories { get; set; }
        public DbSet<LogAction> LogActions { get; set; }
        public DbSet<DatabaseCache> DatabaseCaches { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Application> Applications { get; set; }
        public DbSet<Table>  Tables { get; set; }
        public DbSet<Schema> Schemas { get; set; }
        public DbSet<TableDetail> TableDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<IdentityUserClaim<Guid>>().ToTable("AppUserClaims").HasKey(x => x.Id);
            builder.Entity<IdentityRoleClaim<Guid>>().ToTable("AppRoleClaims").HasKey(x => x.Id);
            builder.Entity<IdentityUserLogin<Guid>>().ToTable("AppUserLogins").HasKey(x => x.UserId);
            builder.Entity<IdentityUserRole<Guid>>().ToTable("AppUserRoles").HasKey(x => new { x.RoleId, x.UserId });
            builder.Entity<IdentityUserToken<Guid>>().ToTable("AppUserTokens").HasKey(x => new { x.UserId });
        }

        /// <summary>
        /// Ghi đè hàm SaveChanges, tự động truyền DateCreated và DateModified
        /// </summary>
        /// <returns></returns>
        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            var entityEntries = ChangeTracker.Entries().Where(e => e.State == EntityState.Modified || e.State == EntityState.Deleted || e.State == EntityState.Added).ToList() ?? new List<EntityEntry>();
            var logHistory = entityEntries.FirstOrDefault(x => x.Entity.GetType().Name == "LogHistory")?.Entity as LogHistory ?? new LogHistory();

            foreach (EntityEntry entityEntry in entityEntries)
            {
                if (entityEntry.Entity is EntityExample entityExample)
                {
                    if (entityEntry.State == EntityState.Added)
                        entityExample.DateCreated = DateTime.Now;
                    entityExample.DateModified = DateTime.Now;
                }
                else if (entityEntry.Entity is DateTracking dateTracking)
                {
                    if (entityEntry.State == EntityState.Added)
                        dateTracking.DateCreated = DateTime.Now;
                    dateTracking.DateModified = DateTime.Now;
                }
                if (entityEntry.Entity is ILogAction)
                    await CreateAuditAsync(entityEntry, logHistory);
            }

            return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private async Task CreateAuditAsync(EntityEntry entityEntry, LogHistory logHistory)
        {
            List<LogAction> logActions = new List<LogAction>();

            foreach (var prop in entityEntry.Properties)
            {
                var comment = ((CommentAttribute)entityEntry.Entity?.GetType()?.GetProperty(prop?.Metadata?.Name ?? "")?.GetCustomAttributes(typeof(CommentAttribute), true).FirstOrDefault()!)?.Comment;
                var originalValue = prop?.OriginalValue?.ToString()?.Trim() ?? "";
                var currentValue = prop?.CurrentValue?.ToString()?.Trim() ?? "";

                if (entityEntry.State == EntityState.Added || entityEntry.State == EntityState.Deleted)
                {
                    if (LogActions != null)
                        await LogActions.AddAsync(new LogAction()
                        {
                            LogHistoryId = logHistory.Id,
                            Action = entityEntry.State.ToString(),
                            UserId = logHistory.UserId,
                            PropertyName = prop?.Metadata?.Name ?? "",
                            ObjectId = logHistory.ObjectId,
                            ObjectName = string.IsNullOrWhiteSpace(comment) ? prop?.Metadata?.Name ?? "" : comment,
                            OldValue = "",
                            NewValue = currentValue,
                            DateCreated = DateTime.Now,
                            DateModified = DateTime.Now,
                        });
                }
                else//Sửa
                {
                    if (prop?.Metadata?.Name == "DateModified") continue;
                    if (originalValue.ToLower() == currentValue.ToLower()) continue;
                    if (LogActions != null)
                        await LogActions.AddAsync(new LogAction()
                        {
                            LogHistoryId = logHistory.Id,
                            Action = entityEntry.State.ToString(),
                            UserId = logHistory.UserId,
                            PropertyName = prop?.Metadata?.Name ?? "",
                            ObjectId = logHistory.ObjectId,
                            ObjectName = string.IsNullOrWhiteSpace(comment) ? prop?.Metadata?.Name ?? "" : comment,
                            OldValue = originalValue,
                            NewValue = currentValue,
                            DateCreated = DateTime.Now,
                            DateModified = DateTime.Now,
                        });
                }
            }
        }
    }
}