using _4Layer.Api.Common.Serialization;

namespace _4Layer.Api.Extensions;

public static class JsonSetup
{
	public static IServiceCollection AddJsonCaseSetup(this IServiceCollection services)
	{
		services
			.AddControllers()
			.AddJsonOptions(options =>
			{
				options.JsonSerializerOptions.PropertyNamingPolicy =
					CaseNamingPolicies.SnakeCase; 
			});

		return services;
	}
}