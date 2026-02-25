using _4Layer.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4Layer.Application.Abstractions.Security
{
	public interface IJwtTokenGenerator
	{
		string GenerateToken(User user);
		RefreshTokenResult GenerateRefreshToken(User user);
		Guid GetUserIdFromToken(string token);
	}
	public record RefreshTokenResult(string Token, DateTime ExpiryDate);
}
