using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace kur
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<Entities>
    {
        public Entities CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var builder = new DbContextOptionsBuilder<Entities>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            builder.UseSqlServer(connectionString, options => options.EnableRetryOnFailure());

            var context = new Entities(builder.Options, configuration);

            return context;
        }
    }
}