
using _4Layer.Api.Common.Responses;
using _4Layer.Domain.Common;
using _4Layer.Api.Common.Helper;
using System.Net;
using System.Text.Json;


namespace _4Layer.Api.Common.Middleware
{
	public class ExceptionMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly ILogger<ExceptionMiddleware> _logger;
		private readonly IWebHostEnvironment _env;
		private readonly IErrorCatalog _errorCatalog;

		public ExceptionMiddleware(
			RequestDelegate next,
			ILogger<ExceptionMiddleware> logger,
			IWebHostEnvironment env, 
			IErrorCatalog errorCatalog)
		{
			_next = next;
			_logger = logger;
			_env = env;
			_errorCatalog = errorCatalog;
		}
		public async Task InvokeAsync(HttpContext context)
		{
			try
			{
				await _next(context);
			}
			catch (FluentValidation.ValidationException ex)
			{
				_logger.LogWarning("Validation error");

				await HandleExceptionFluentValidationAsync(context, ex);
			}
			catch (AppError ex)
			{
				_logger.LogWarning("Business rule violated: {Message}", ex.Message);
				await HandleExceptionBussinessAsync(context, ex);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Unhandled Exception");
				await HandleExceptionInternalAsync(context, ex);
			}
		}
		private async Task HandleExceptionFluentValidationAsync(
			HttpContext context,
			FluentValidation.ValidationException ex)
		{
			context.Response.Clear();
			context.Response.ContentType = "application/json";
			context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

			var errors = ex.Errors.Select(e =>
			{
				var errorCode = string.IsNullOrWhiteSpace(e.ErrorCode)
					? e.ErrorMessage
					: e.ErrorCode;

				var catalog = _errorCatalog.Get(errorCode);

				return new ValidationError
				{
					Field = e.PropertyName,
					Code = catalog?.error_code ?? errorCode ?? "VALIDATION_ERROR",
					Description = catalog?.details
								  ?? catalog?.message
								  ?? e.ErrorMessage
				};
			}).ToList();

			var response = ErrorResponse.Create(
				context.Response.StatusCode,
				"Invalid request body",
				AppError.GetErrorPhrase(ErrorStatus.BadRequest),
				errors
			);

			await WriteResponseAsync(context, response);
		}
		private async Task HandleExceptionInternalAsync(HttpContext context, Exception ex)
		{
			context.Response.Clear();
			context.Response.ContentType = "application/json";
			context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

			var message = _env.IsDevelopment()
				? ex.Message
				: "An unexpected error occurred.";

			var response = ErrorResponse.Create(
				context.Response.StatusCode,
				message,
				"Internal Server Error",
				null
			);

			await WriteResponseAsync(context, response);
		}

		private async Task HandleExceptionBussinessAsync(HttpContext context, AppError ex)
		{
			context.Response.Clear();
			context.Response.ContentType = "application/json";
			context.Response.StatusCode = (int)ex.Status;

			var response = ErrorResponse.Create(
				context.Response.StatusCode,
				ex.Message,
				ex.ErrorPhrase,
				null
			);

			await WriteResponseAsync(context, response);
		}

		private static Task WriteResponseAsync(HttpContext context, object response)
		{
			return context.Response.WriteAsJsonAsync(response);
		}
	}
}
