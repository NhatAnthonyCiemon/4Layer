using _4Layer.Application.Abstractions.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _4Layer.Application.Abstractions.Security;
using _4Layer.Application.Users.LogIn.Response;
using _4Layer.Domain.Entities;
using _4Layer.Domain.Repositories;
using System.Threading;

namespace _4Layer.Application.Services
{
	public class AuthService : IAuthService
	{
		private readonly IJwtTokenGenerator _jwtTokenGenerator;
		private readonly IPasswordHasher _passwordHasher;
		private readonly IUserRepository _userRepository;

		public AuthService(
						IUserRepository userRepository,
									IPasswordHasher passwordHasher,
												IJwtTokenGenerator jwtTokenGenerator
			)
		{
			_userRepository = userRepository;
			_passwordHasher = passwordHasher;
			_jwtTokenGenerator = jwtTokenGenerator;
		}

		public async Task<LoginResult> GenerateTokensAsync(User user, CancellationToken ct)
		{
			var refeshTokenResult = _jwtTokenGenerator.GenerateRefreshToken(user);
			await _userRepository.CreateSession(new Session(user.Id, _passwordHasher.Hash(refeshTokenResult.Token), refeshTokenResult.ExpiryDate, "126.7.9.9", null), ct);
			return new LoginResult(AccessToken: _jwtTokenGenerator.GenerateToken(user), RefreshToken: refeshTokenResult.Token, Id: user.Id);
		}
	}
}
