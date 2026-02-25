using System.Text;
using _4Layer.Api.Common.CurrentUser;
using _4Layer.Api.Common.Responses;
using _4Layer.Application.Abstractions.Security;
using _4Layer.Domain.Common;
using _4Layer.Infrastructure.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;

namespace _4Layer.Api.Extensions;

public static class AuthenticationExtensions
{
	public static IServiceCollection AddJwtAuthentication(
		this IServiceCollection services,
		IConfiguration configuration)
	{
		var jwtSettings = configuration
			.GetSection("JwtSettings")
			.Get<JwtSettings>();

		services.AddAuthentication(options =>
		{
			options.DefaultAuthenticateScheme =
				JwtBearerDefaults.AuthenticationScheme;

			options.DefaultChallengeScheme =
				JwtBearerDefaults.AuthenticationScheme;
		})
		.AddJwtBearer(options =>
		{
			options.TokenValidationParameters =
				new TokenValidationParameters
				{
					ValidateIssuer = true,
					ValidateAudience = true,
					ValidateLifetime = true,
					ValidateIssuerSigningKey = true,

					ValidIssuer = jwtSettings!.Issuer,
					ValidAudience = jwtSettings.Audience,
					IssuerSigningKey = new SymmetricSecurityKey(
						Encoding.UTF8.GetBytes(jwtSettings.SecretKey))
				};
			options.Events = new JwtBearerEvents
			{
				OnChallenge = async context =>
				{
					context.HandleResponse();
					var statusCode = ErrorStatus.Unauthorized;
					var response = ErrorResponse.Create(
						(int)statusCode,
						"Invalid or missing token.",
						AppError.GetErrorPhrase(statusCode),
						null
					);

					context.Response.StatusCode = (int)statusCode;
					context.Response.ContentType = "application/json";

					await context.Response.WriteAsJsonAsync(response);
				},

				OnForbidden = async context =>
				{
					var statusCode = ErrorStatus.Forbidden;
					var response = ErrorResponse.Create(
						(int)statusCode,
						"You do not have permission to access this resource.",
						AppError.GetErrorPhrase(statusCode),
						null
					);

					context.Response.StatusCode = (int)statusCode;
					context.Response.ContentType = "application/json";

					await context.Response.WriteAsJsonAsync(response);
				}
			};
		});

		services.AddAuthorization(option =>
		{
			option.FallbackPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
		});

		services.AddHttpContextAccessor();
		services.AddScoped<ICurrentUser, CurrentUser>();

		return services;
	}
}