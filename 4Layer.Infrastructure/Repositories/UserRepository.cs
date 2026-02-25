using _4Layer.Domain.Repositories;
using _4Layer.Domain.Entities;
using _4Layer.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace _4Layer.Infrastructure.Repositories
{
	public class UserRepository : IUserRepository
	{
		private readonly AppDbContext _context;
		public UserRepository(AppDbContext context)
		{
			_context = context;
		}
		public async Task AddUser(User user, CancellationToken cancellationToken)
		{
			await _context.Users.AddAsync(user, cancellationToken);
			await _context.SaveChangesAsync(cancellationToken);
		}
		public async Task CreateSession(Session session, CancellationToken cancellationToken)
		{
			await _context.Sessions.AddAsync(session, cancellationToken);
			await _context.SaveChangesAsync(cancellationToken);
		}
		public async Task<bool> IsEmailExist(string email, CancellationToken cancellationToken)
		{
			return await _context.Users
								.AnyAsync(u => u.Email == email, cancellationToken);
		}
		public async Task<User?> GetUserByEmail(string email, CancellationToken cancellationToken)
		{
			return await _context.Users
								.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
		}
		public async Task<User?> GetUserById(Guid id, CancellationToken cancellationToken)
		{
			return await _context.Users
								.FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
		}
		public async Task<List<Session>> GetRefreshTokenById(Guid id, CancellationToken cancellationToken)
		{
			return await _context.Users
								.Where(u => u.Id == id)
								.SelectMany(u => u.Sessions).ToListAsync(cancellationToken);
		}

		public async Task RevokeRefreshToken(Guid Id, CancellationToken cancellationToken)
		{
			var session = await _context.Sessions.Where(s => s.Id == Id).FirstOrDefaultAsync(cancellationToken);
			if(session != null)
			{
				session.Revoke();
				await _context.SaveChangesAsync(cancellationToken);
			}
			
		}

	}
}
