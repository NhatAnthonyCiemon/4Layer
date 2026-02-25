using _4Layer.Api.Common.Middleware;

namespace _4Layer.Api.Extensions;

public static class MiddlewareSetup
{
	public static IApplicationBuilder UseCustomMiddlewares(
		this IApplicationBuilder app)
	{
		app.UseMiddleware<RequestLoggingMiddleware>();
		app.UseMiddleware<ExceptionMiddleware>();
		return app;
	}
}