namespace AosAdjutant.Api.Common;

public abstract class AuditableEntity
{
    public DateTimeOffset CreatedAt { get; set; }
    public int? CreatedByUserId { get; set; }

    public DateTimeOffset? ModifiedAt { get; set; }
    public int? ModifiedByUserId { get; set; }
}
