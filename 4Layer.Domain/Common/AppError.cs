
namespace _4Layer.Domain.Common;

public class AppError : Exception
{
	public ErrorStatus Status { get; }
	public string ErrorPhrase { get; }

	public AppError(
		string message,
		ErrorStatus status = ErrorStatus.InternalServerError)
		: base(message)
	{
		Status = status;
		ErrorPhrase = GetErrorPhrase(status);
	}

	public static string GetErrorPhrase(ErrorStatus status)
	{
		return status switch
		{
			ErrorStatus.BadRequest => "Bad Request",
			ErrorStatus.Unauthorized => "Unauthorized",
			ErrorStatus.Forbidden => "Forbidden",
			ErrorStatus.NotFound => "Not Found",
			ErrorStatus.Conflict => "Conflict",
			ErrorStatus.InternalServerError => "Internal Server Error",
			_ => "Error"
		};
	}
}