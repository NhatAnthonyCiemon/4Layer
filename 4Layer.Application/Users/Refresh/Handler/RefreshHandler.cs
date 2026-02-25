using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _4Layer.Application.Users.LogIn.Response;
using MediatR;
using _4Layer.Application.Users.Refresh.Commands;
using _4Layer.Application.Abstractions.Security;
using _4Layer.Domain.Repositories;
using _4Layer.Domain.Common;
using _4Layer.Application.Abstractions.Services;
using System.Text.Json;

namespace _4Layer.Application.Users.Refresh.Handler
{
	public class RefreshHandler : IRequestHandler<RefreshCommand, LoginResult>
	{
		private readonly IJwtTokenGenerator _jwtTokenGenerator;
		private readonly IUserRepository _userRepository;
		private readonly IPasswordHasher _passwordHasher;
		private readonly IAuthService _authService;

		public RefreshHandler(IJwtTokenGenerator jwtTokenGenerator, IUserRepository userRepository, IPasswordHasher passwordHasher, IAuthService authService)
		{
			_jwtTokenGenerator = jwtTokenGenerator;
			_userRepository = userRepository;
			_passwordHasher = passwordHasher;
			_authService = authService;
		}

		public async Task<LoginResult> Handle(RefreshCommand request, CancellationToken cancellationToken)
		{
			var id = _jwtTokenGenerator.GetUserIdFromToken(request.RefreshToken);
			var user = await _userRepository.GetUserById(id, cancellationToken);

			if (user == null || !user.IsActive)
			{
				throw new AppError("Invalid user.", ErrorStatus.Unauthorized);
			}

			var sessions = await _userRepository.GetRefreshTokenById(id, cancellationToken);
			foreach (var s in sessions)
			{
				var x = _passwordHasher.Verify(request.RefreshToken, s.RefreshToken);
			}
			Console.WriteLine($"[REFRESH] Total sessions for user: {sessions.Count()}");
			Console.WriteLine($"[REFRESH] Incoming token (first 50 chars): {request.RefreshToken.Substring(0, Math.Min(50, request.RefreshToken.Length))}...");

			var session = sessions.FirstOrDefault(s => _passwordHasher.Verify(request.RefreshToken, s.RefreshToken));

			if (session != null)
			{
				Console.WriteLine($"[REFRESH] Matched Session ID: {session.Id}");
				Console.WriteLine($"[REFRESH] Session IsRevoked: {session.IsRevoked}");
				Console.WriteLine($"[REFRESH] Session ExpiresAt: {session.ExpiresAt}");
				Console.WriteLine($"[REFRESH] Session IsActive (method): {session.IsActive()}");
				Console.WriteLine($"[REFRESH] Current UTC: {DateTime.UtcNow}");
			}
			else
			{
				Console.WriteLine($"[REFRESH] No matching session found!");
				foreach (var s in sessions.Take(3))
				{
					Console.WriteLine($"[REFRESH] Session {s.Id} - IsRevoked: {s.IsRevoked}, ExpiresAt: {s.ExpiresAt}");
				}
			}

			if (session == null || session.ExpiresAt < DateTime.UtcNow || !session.IsActive())
				throw new AppError("Invalid refresh token.", ErrorStatus.Unauthorized);

			await _userRepository.RevokeRefreshToken(session.Id, cancellationToken);
			Console.WriteLine($"[REFRESH] Session {session.Id} revoked");

			var result = await _authService.GenerateTokensAsync(user, cancellationToken);
			Console.WriteLine($"[REFRESH] New token generated (first 50 chars): {result.RefreshToken.Substring(0, Math.Min(50, result.RefreshToken.Length))}...");

			return result;
		}

	}
}
