using _4Layer.Api.Common.Responses;
using Microsoft.AspNetCore.Mvc;
using _4Layer.Domain.Common;

namespace _4Layer.Api.Common.Helper
{
	public static class StatusHelper
	{
		public static IActionResult Success<T>(
			T data,
			string message = "Success",
			SuccessStatus status = SuccessStatus.Success)
		{
			var statusCode = (int)status;

			return new ObjectResult(
				SuccessResponse<T>.Create(data, message, statusCode))
			{
				StatusCode = statusCode
			};
		}

		public static IActionResult Error(
			string message,
			ErrorStatus status = ErrorStatus.InternalServerError,
			List<ValidationError>? errors = null)
		{
			var statusCode = (int)status;

			return new ObjectResult(
				ErrorResponse.Create(
					statusCode,
					message,
					AppError.GetErrorPhrase(status),
					errors))
			{
				StatusCode = statusCode
			};
		}

	}
}