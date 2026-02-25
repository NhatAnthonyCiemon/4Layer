namespace _4Layer.Api.Common.Helper
{
	public class ErrorDefinition
	{
		public string error_code { get; set; } = default!;
		public string message { get; set; } = default!;
		public string details { get; set; } = default!;
	}
}
