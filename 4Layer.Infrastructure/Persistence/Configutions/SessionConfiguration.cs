using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using _4Layer.Domain.Entities;
using System.Reflection.Emit;

namespace _4Layer.Infrastructure.Persistence.Configurations;

public class SessionConfiguration : IEntityTypeConfiguration<Session>
{
	public void Configure(EntityTypeBuilder<Session> builder)
	{
		builder.ToTable("Sessions");

		builder.HasKey(x => x.Id);

		builder.Property(x => x.RefreshToken)
			   .IsRequired()
			   .HasMaxLength(500);

		builder.Property(x => x.ExpiresAt)
				.IsRequired();

		builder
			.HasOne(s => s.User)
			.WithMany(u => u.Sessions)
			.HasForeignKey(s => s.UserId);
	}
}