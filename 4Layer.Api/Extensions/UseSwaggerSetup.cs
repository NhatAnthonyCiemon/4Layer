

namespace _4Layer.Api.Extensions;

public static class UseSwaggerSetup
{
	public static IApplicationBuilder SetUpUseSwagger(
		this IApplicationBuilder app)
	{
		app.UseSwagger();
		app.UseSwaggerUI();
		return app;
	}
}