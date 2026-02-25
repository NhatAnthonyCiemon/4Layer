using _4Layer.Domain.Entities;


namespace _4Layer.Domain.Repositories
{
	public interface IUserRepository
	{
		Task<bool> IsEmailExist(string email, CancellationToken cancellationToken);
		Task AddUser(User user, CancellationToken cancellationToken);
		Task RevokeRefreshToken(Guid Id, CancellationToken cancellationToken);
		Task<User?> GetUserByEmail(string email, CancellationToken cancellationToken);
		Task CreateSession(Session session, CancellationToken cancellationToken);
		Task<User?> GetUserById(Guid id, CancellationToken cancellationToken);
		Task<List<Session>> GetRefreshTokenById(Guid id, CancellationToken cancellationToken);
	}
}
