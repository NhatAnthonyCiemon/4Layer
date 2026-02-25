using System.Text.Json;
using System.Text.RegularExpressions;

namespace _4Layer.Api.Common.Serialization;

public static class CaseNamingPolicies
{
	public static JsonNamingPolicy CamelCase => JsonNamingPolicy.CamelCase;

	public static JsonNamingPolicy SnakeCase => new SnakeCaseNamingPolicy();

	private class SnakeCaseNamingPolicy : JsonNamingPolicy
	{
		public override string ConvertName(string name)
		{
			return Regex
				.Replace(name, "([a-z0-9])([A-Z])", "$1_$2")
				.ToLower();
		}
	}
}