using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace _4Layer.Infrastructure.Persistence;

public class AppDbContextFactory
	: IDesignTimeDbContextFactory<AppDbContext>
{
	public AppDbContext CreateDbContext(string[] args)
	{
		var basePath = Path.Combine(
			Directory.GetCurrentDirectory(),
			"../4Layer.Api"
		);

		var configuration = new ConfigurationBuilder()
			.SetBasePath(basePath)
			.AddJsonFile("appsettings.json", optional: false)
			.AddJsonFile("appsettings.Development.json", optional: true)
			.AddEnvironmentVariables()
			.Build();

		var connectionString =
			configuration.GetConnectionString("DefaultConnection")
			?? throw new InvalidOperationException(
				"DefaultConnection not found.");

		var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
		optionsBuilder.UseNpgsql(connectionString);

		return new AppDbContext(optionsBuilder.Options);
	}
}