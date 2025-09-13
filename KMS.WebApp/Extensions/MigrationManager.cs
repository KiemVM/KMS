using Microsoft.EntityFrameworkCore;
using KMS.Data;
using KMS.Data.SeedWorks;

namespace KMS.WebApp.Extensions
{
    public static class MigrationManager
    {
        public static WebApplication MigrateDatabase(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            using var context = scope.ServiceProvider.GetRequiredService<KMSContext>();
            context.Database.Migrate();
            new DataSeeder().SeedAsync(context).Wait();
            return app;
        }
    }
}