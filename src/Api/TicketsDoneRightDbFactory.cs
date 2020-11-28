using System.IO;
using NetCoreExample.Server.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using NetCoreExample.Server.Configuration;

namespace NetCoreExample.Server.Api
{
    public class NetCoreExampleDbFactory : IDesignTimeDbContextFactory<NetCoreExampleDbContext>
    {
        public NetCoreExampleDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddJsonFile("appsettings.Development.json")
                .AddJsonFile("appsettings.Staging.json")
                .Build();

            var builder = new DbContextOptionsBuilder<NetCoreExampleDbContext>();

            var connectionString = configuration.DbConnectionString();
            builder.UseMySql(connectionString, (options) => { 
                options.EnableRetryOnFailure();
            });

            return new NetCoreExampleDbContext(builder.Options);
        }
    }
}
