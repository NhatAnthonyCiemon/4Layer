using Microsoft.AspNetCore.Mvc;
using _4Layer.Api.Common.Helper;
using _4Layer.Api.Common.Responses;
using _4Layer.Domain.Common;

namespace _4Layer.Api.Extensions
{
	public static class ValidationSetup
	{
		public static void AddValidationSetup(this IServiceCollection services)
		{

			services.Configure<ApiBehaviorOptions>(options =>
			{
				options.InvalidModelStateResponseFactory = context =>
				{
	
					var errors = context.ModelState
						.Where(e => e.Value != null && e.Value.Errors.Count > 0)
						.SelectMany(e => e.Value!.Errors.Select(err => new ValidationError
						{
							Field = e.Key,
							Code = "INVALID_TYPE",
							Description = err.ErrorMessage
						}))
						.ToList();

					return StatusHelper.Error(
						message: "Invalid request body",
						status: ErrorStatus.BadRequest,
						errors
					);
				};
			});
		}
	}
}
