using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using _4Layer.Domain.Entities;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
	public void Configure(EntityTypeBuilder<User> builder)
	{
		builder.ToTable("Users");

		builder.HasKey(x => x.Id);

		builder.Property(x => x.Email)
			   .IsRequired();

		builder.Property(x => x.PasswordHash)
			   .IsRequired();

		builder
			.HasMany(u => u.Sessions)
			.WithOne(s => s.User)
			.HasForeignKey(s => s.UserId)
			.OnDelete(DeleteBehavior.Cascade);

	}
}