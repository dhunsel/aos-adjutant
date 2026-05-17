using AosAdjutant.Api.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace AosAdjutant.Api.Database;

public sealed class AuditSaveChangesInterceptor(ICurrentUser currentUser, TimeProvider timeProvider)
    : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default
    )
    {
        if (eventData.Context is not null)
            ApplyAudit(eventData.Context);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result
    )
    {
        if (eventData.Context is not null)
            ApplyAudit(eventData.Context);

        return base.SavingChanges(eventData, result);
    }

    private void ApplyAudit(DbContext context)
    {
        var now = timeProvider.GetUtcNow();
        var userId = currentUser.UserId;

        foreach (var entry in context.ChangeTracker.Entries<AuditableEntity>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = now;
                entry.Entity.CreatedByUserId = userId;
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Entity.ModifiedAt = now;
                entry.Entity.ModifiedByUserId = userId;

                // Created by columns cannot change
                entry.Property(nameof(AuditableEntity.CreatedAt)).IsModified = false;
                entry.Property(nameof(AuditableEntity.CreatedByUserId)).IsModified = false;
            }
        }
    }
}
