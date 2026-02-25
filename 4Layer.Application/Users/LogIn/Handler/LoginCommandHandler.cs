using _4Layer.Domain.Repositories;
using _4Layer.Application.Abstractions.Security;
using _4Layer.Application.Users.LogIn.Commands;
using MediatR;
using _4Layer.Domain.Common;
using _4Layer.Application.Users.LogIn.Response;
using _4Layer.Domain.Entities;
using _4Layer.Application.Abstractions.Services;


namespace _4Layer.Application.Users.LogIn.Handler
{
	public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResult>
	{
		private readonly IUserRepository _userRepository;
		private readonly IPasswordHasher _passwordHasher;
		private readonly IAuthService _authService;

		public LoginCommandHandler(
						IUserRepository userRepository,
									IPasswordHasher passwordHasher,
												IAuthService authService
			)
		{
			_userRepository = userRepository;
			_authService = authService;
			_passwordHasher = passwordHasher;
		}

		public async Task<LoginResult> Handle(LoginCommand request, CancellationToken cancellationToken)
		{
			var user = await _userRepository.GetUserByEmail(request.Email, cancellationToken);

			if (user == null || !user.IsActive || !_passwordHasher.Verify(request.Password, user.PasswordHash))
				throw new AppError("Invalid email or password.", ErrorStatus.Unauthorized);
			return await _authService.GenerateTokensAsync(user, cancellationToken);
		}

	}
}
