using _4Layer.Application.Abstractions.Security;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace _4Layer.Api.Common.CurrentUser
{
	public class CurrentUser : ICurrentUser
	{
		private readonly IHttpContextAccessor _httpContextAccessor;

		public CurrentUser(IHttpContextAccessor httpContextAccessor)
		{
			_httpContextAccessor = httpContextAccessor;
		}

		public Guid UserId
		{
			get
			{
				var userId = _httpContextAccessor.HttpContext?
					.User?
					.FindFirstValue(ClaimTypes.NameIdentifier)
					?? _httpContextAccessor.HttpContext?
					.User?
					.FindFirstValue(JwtRegisteredClaimNames.Sub);

				return Guid.Parse(userId!);
			}
		}

		public string? Email =>
			_httpContextAccessor.HttpContext?
				.User?
				.FindFirstValue(ClaimTypes.Email);
	}
}
