using AosAdjutant.Api.Common;
using AosAdjutant.Api.Features.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AosAdjutant.Api.Database;

public static class EntityTypeBuilderExtensions
{
    public static void ConfigureAuditColumns<TEntity>(this EntityTypeBuilder<TEntity> builder)
        where TEntity : AuditableEntity
    {
        builder.Property(a => a.CreatedAt).HasColumnName("created_at");
        builder.Property(a => a.CreatedByUserId).HasColumnName("created_by_user_id");
        builder.Property(a => a.ModifiedAt).HasColumnName("modified_at");
        builder.Property(a => a.ModifiedByUserId).HasColumnName("modified_by_user_id");

        builder
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(a => a.CreatedByUserId)
            .OnDelete(DeleteBehavior.Restrict);
        builder
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(a => a.ModifiedByUserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
