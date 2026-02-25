using System.Text.Json;

namespace _4Layer.Api.Common.Helper
{
	public interface IErrorCatalog
	{
		ErrorDefinition? Get(string code);
	}
	public class ErrorCatalog : IErrorCatalog
	{
		private readonly Dictionary<string, ErrorDefinition> _errors;

		public ErrorCatalog(IWebHostEnvironment env)
		{
			var path = Path.Combine(env.ContentRootPath, "Resources", "error_en.json");
			var json = File.ReadAllText(path);

			_errors = JsonSerializer.Deserialize<Dictionary<string, ErrorDefinition>>(json)
					  ?? new Dictionary<string, ErrorDefinition>();
		}

		public ErrorDefinition? Get(string code)
		{
			return _errors.TryGetValue(code, out var value) ? value : null;
		}
	}
}
