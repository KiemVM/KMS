using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace KMS.Data
{
    public class KMSContextFactory : IDesignTimeDbContextFactory<KMSContext>
    {
        public KMSContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("AppSettings.json")
                .Build();
            var builder = new DbContextOptionsBuilder<KMSContext>();
            builder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            return new KMSContext(builder.Options);
        }
    }
}