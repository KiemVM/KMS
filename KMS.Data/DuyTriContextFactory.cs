using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace KMS.Data
{
    public class SaaSContextFactory : IDesignTimeDbContextFactory<SaaSContext>
    {
        public SaaSContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("AppSettings.json")
                .Build();
            var builder = new DbContextOptionsBuilder<SaaSContext>();
            builder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            return new SaaSContext(builder.Options);
        }
    }
}