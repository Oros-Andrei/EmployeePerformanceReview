using EmployeeControlService.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace EmployeePerformanceReview.Entities
{
    public class ContextFactory : IDesignTimeDbContextFactory<Db>
    {
        public Db CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build(); ;

            var builder = new DbContextOptionsBuilder<Db>();
            var connectionString = configuration.GetConnectionString("ConnectionString");
            builder.UseSqlServer(connectionString, b => b.MigrationsAssembly("EmployeePerformanceReview"));

            return new Db(builder.Options);
        }
    }
}