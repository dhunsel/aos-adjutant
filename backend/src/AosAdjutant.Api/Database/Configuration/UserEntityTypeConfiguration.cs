using AosAdjutant.Api.Features.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AosAdjutant.Api.Database.Configuration;

public sealed class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("user");
        builder.Property(u => u.UserId).HasColumnName("user_id");
        builder.Property(u => u.PublicId).HasColumnName("public_id");
        builder.Property(u => u.Username).HasColumnName("username").HasMaxLength(250);
        builder.Property(u => u.Email).HasColumnName("email").HasMaxLength(250);
        builder
            .Property(u => u.IdentityProvider)
            .HasColumnName("identity_provider")
            .HasMaxLength(250);
        builder.Property(u => u.Subject).HasColumnName("subject").HasMaxLength(250);

        builder.HasKey(u => u.UserId);
        builder.HasIndex(u => u.PublicId).IsUnique();
        builder.HasIndex(u => new { u.IdentityProvider, u.Subject }).IsUnique();
    }
}
