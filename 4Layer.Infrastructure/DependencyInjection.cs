using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using _4Layer.Infrastructure.Persistence;
using _4Layer.Domain.Repositories;
using _4Layer.Infrastructure.Repositories;
using _4Layer.Application.Abstractions.Security;
using _4Layer.Infrastructure.Security;

namespace _4Layer.Infrastructure;

public static class DependencyInjection
{
	public static IServiceCollection AddInfrastructure(
		this IServiceCollection services,
		IConfiguration configuration)
	{
		services.AddDbContext<AppDbContext>(options =>
			options.UseNpgsql(
				configuration.GetConnectionString("DefaultConnection")));

		// Register repositories
		services.AddScoped<IUserRepository, UserRepository>();

		// Register other services, e.g., UnitOfWork, if needed
		services.AddScoped<IPasswordHasher, PasswordHasher>();
		// Register JWT token generator
		services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
		services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();



		return services;
	}
}