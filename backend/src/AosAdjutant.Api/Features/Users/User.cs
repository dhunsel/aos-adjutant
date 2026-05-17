namespace AosAdjutant.Api.Features.Users;

public sealed class User
{
    public int UserId { get; set; }
    public Guid PublicId { get; set; }
    public required string Username { get; set; }
    public required string Email { get; set; }
    public required string IdentityProvider { get; set; }
    public required string Subject { get; set; }
}
