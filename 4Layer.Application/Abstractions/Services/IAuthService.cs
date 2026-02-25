using _4Layer.Application.Users.LogIn.Response;
using _4Layer.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4Layer.Application.Abstractions.Services
{
	public interface IAuthService
	{
		Task<LoginResult> GenerateTokensAsync(User user, CancellationToken ct);
	}
}
