namespace _4Layer.Api.Common.Responses
{
	public class ValidationError
	{
		public string Code { get; set; } = default!;
		public string Field { get; set; } = default!;
		public string Description { get; set; } = default!;
	}
	public class ErrorResponse
	{
		public int Code { get; set; }
		public string Message { get; set; } = default!;
		public string Error { get; set; } = default!;
		public List<ValidationError>? Errors { get; set; }
		public static ErrorResponse Create(int statusCode, string message, string error, List<ValidationError>? errors = null)
		{
			return new ErrorResponse
			{
				Code = statusCode,
				Message = message,
				Error = error,
				Errors = errors
			};
		}
	}
}
