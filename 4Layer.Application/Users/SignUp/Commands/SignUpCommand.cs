using _4Layer.Domain.Enums;
using MediatR;

public sealed class CreateUserCommand : IRequest<Guid>
{
	public string Email { get; init; } = null!;
	public string Password { get; init; } = null!;

	public string? PhoneNumber { get; init; }
	public string? AvatarUrl { get; init; }

	public UserRole Role { get; init; }
}