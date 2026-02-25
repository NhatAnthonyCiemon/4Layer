using _4Layer.Domain.Common;
using _4Layer.Domain.Enums;

namespace _4Layer.Domain.Entities;

public class User : BaseEntity
{
	public string Email { get; private set; } = null!;
	public string PasswordHash { get; private set; } = null!;

	public string? PhoneNumber { get; private set; }
	public string? AvatarUrl { get; private set; }

	public bool IsActive { get; private set; }
	public UserRole Role { get; private set; }

	private readonly List<Session> _sessions = new();
	public IReadOnlyCollection<Session> Sessions => _sessions.AsReadOnly();

	private User() { } // For EF

	public User(
		string email,
		string passwordHash,
		string? phoneNumber,
		string? avatarUrl,
		UserRole role)
	{
		Email = email;
		PasswordHash = passwordHash;
		PhoneNumber = phoneNumber;
		AvatarUrl = avatarUrl;
		Role = role;
		IsActive = true;
	}

	public void ChangePassword(string newHash)
	{
		PasswordHash = newHash;
	}

	public void Deactivate()
	{
		IsActive = false;
	}

	public void Activate()
	{
		IsActive = true;
	}

	public Session AddSession(Session session)
	{
		_sessions.Add(session);
		return session;
	}
}