using Microsoft.EntityFrameworkCore;
using _4Layer.Domain.Entities;
using _4Layer.Infrastructure.Helper;

namespace _4Layer.Infrastructure.Persistence
{
	public class AppDbContext : DbContext
	{
		public DbSet<User> Users { get; set; } = null!;
		public DbSet<Session> Sessions { get; set; } = null!;

		public AppDbContext(DbContextOptions<AppDbContext> options)
			: base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
		}
		public override Task<int> SaveChangesAsync(
			CancellationToken cancellationToken = default)
		{
			ApplyAuditFields.ApplyAuditFieldsToEntities(ChangeTracker.Entries());
			return base.SaveChangesAsync(cancellationToken);
		}

		
	}
}
