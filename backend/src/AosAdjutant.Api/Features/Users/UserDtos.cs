namespace AosAdjutant.Api.Features.Users;

public sealed record ExternalIdentity(
    string Provider,
    string Subject,
    string Username,
    string Email
);
