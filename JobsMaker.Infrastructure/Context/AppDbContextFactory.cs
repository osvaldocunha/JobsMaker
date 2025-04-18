using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace JobsMaker.Infrastructure.Context;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
    .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../JobsMaker.API"))
    .AddJsonFile("appsettings.json")
    .Build();

        var connectionString = configuration.GetConnectionString("DefaultConnection");

        //var serverVersion = ServerVersion.AutoDetect(connectionString); // Automatically detect MySQL version
        var serverVersion = new MySqlServerVersion(new Version(8, 0, 0));

        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseMySql(connectionString, serverVersion);

        return new AppDbContext(optionsBuilder.Options);
    }
}