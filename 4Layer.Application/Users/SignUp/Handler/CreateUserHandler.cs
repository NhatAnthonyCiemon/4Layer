using _4Layer.Application.Abstractions.Security;
using _4Layer.Domain.Common;
using _4Layer.Domain.Entities;
using _4Layer.Domain.Repositories;
using MediatR;

namespace _4Layer.Application.Users.Handlers;

public class CreateUserHandler
	: IRequestHandler<CreateUserCommand, Guid>
{
	private readonly IUserRepository _userRepository;
	private readonly IPasswordHasher _passwordHasher;

	public CreateUserHandler(IUserRepository userRepository, IPasswordHasher passwordHasher)
	{
		_userRepository = userRepository;
		_passwordHasher = passwordHasher;
	}

	public async Task<Guid> Handle(
		CreateUserCommand request,
		CancellationToken cancellationToken)
	{
		var isExist = await _userRepository
			.IsEmailExist(request.Email, cancellationToken);

		if (isExist)
			throw new AppError("Email already exists.", ErrorStatus.Conflict);

		var passwordHash = _passwordHasher.Hash(request.Password);

		var user = new User(request.Email,passwordHash, request.PhoneNumber,request.AvatarUrl, request.Role);

		await _userRepository.AddUser(user, cancellationToken);

		return user.Id;
	}
}