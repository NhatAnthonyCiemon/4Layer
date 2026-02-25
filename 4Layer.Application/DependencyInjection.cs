using FluentValidation;
using MediatR;
using System.Reflection;
using _4Layer.Application.Common.Behaviors;
using Microsoft.Extensions.DependencyInjection;
using _4Layer.Application.Abstractions.Services;
using _4Layer.Application.Services;

public static class DependencyInjection
{
	public static IServiceCollection AddApplication(this IServiceCollection services)
	{
		// Register MediatR
		services.AddMediatR(cfg =>
			cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

		// Register Validators
		services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

		// Register Pipeline
		services.AddTransient(typeof(IPipelineBehavior<,>),
							  typeof(ValidationBehavior<,>));

		// Register Services
		services.AddScoped<IAuthService, AuthService>();

		return services;
	}
}