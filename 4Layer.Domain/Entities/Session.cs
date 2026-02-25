using System.Text.Json.Serialization;
using _4Layer.Domain.Common;

namespace _4Layer.Domain.Entities;

public class Session : BaseEntity
{
	public Guid UserId { get; private set; }

	public string RefreshToken { get; private set; } = null!;
	public DateTime ExpiresAt { get; private set; }

	public string IpAddress { get; private set; } = null!;
	public string? UserAgent { get; private set; }

	public bool IsRevoked { get; private set; }
	public DateTime? RevokedAt { get; private set; }
	[JsonIgnore]
	public User User { get; private set; } = null!;

	private Session() { } // For EF

	public Session(
		Guid userId,
		string refreshToken,
		DateTime expiresAt,
		string ipAddress,
		string? userAgent)
	{
		UserId = userId;
		RefreshToken = refreshToken;
		ExpiresAt = expiresAt;
		IpAddress = ipAddress;
		UserAgent = userAgent;
		IsRevoked = false;
	}

	public void Revoke()
	{
		IsRevoked = true;
		RevokedAt = DateTime.UtcNow;
	}

	public bool IsExpired()
		=> DateTime.UtcNow >= ExpiresAt;

	public bool IsActive()
		=> !IsRevoked && !IsExpired();
}