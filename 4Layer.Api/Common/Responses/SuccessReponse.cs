using System.Net;

namespace _4Layer.Api.Common.Responses
{
	public class SuccessResponse<T>
	{
		public int Code { get; set; }
		public string Message { get; set; } = default!;
		public T Data { get; set; } = default!;
		public static SuccessResponse<T> Create(T data, string message = "Success", int code = 200)
		{
			return new SuccessResponse<T>
			{
				Code = code,
				Message = message,
				Data = data
			};
		}
	}
}
