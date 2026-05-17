using AosAdjutant.Api.Database;
using AosAdjutant.Api.Features.Users;
using AosAdjutant.IntegrationTests.Fixture;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AosAdjutant.IntegrationTests.Features.Users;

public class UserServiceTests(ApiFactory factory) : EndpointTestsBase(factory)
{
    private readonly ApiFactory _factory = factory;

    private static ExternalIdentity Identity(
        string subject = "sub-1",
        string provider = "pocket-id",
        string username = "alice",
        string email = "alice@example.com"
    ) => new(provider, subject, username, email);

    private async Task<User> ResolveAsync(ExternalIdentity identity)
    {
        using var scope = _factory.Services.CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<UserService>();
        return await service.ResolveExternalIdentity(identity);
    }

    private async Task<int> CountUsersAsync()
    {
        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        return await context.Users.CountAsync();
    }

    private async Task<User> GetUserAsync(string subject, string provider)
    {
        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        return await context
            .Users.AsNoTracking()
            .FirstAsync(u => u.Subject == subject && u.IdentityProvider == provider);
    }

    [Fact]
    public async Task CreatesUser_WhenIdentityIsNew()
    {
        var identity = Identity();

        var user = await ResolveAsync(identity);

        Assert.True(user.UserId > 0);
        Assert.NotEqual(Guid.Empty, user.PublicId);
        Assert.Equal(identity.Subject, user.Subject);
        Assert.Equal(identity.Provider, user.IdentityProvider);
        Assert.Equal(identity.Username, user.Username);
        Assert.Equal(identity.Email, user.Email);
        Assert.Equal(1, await CountUsersAsync());
    }

    [Fact]
    public async Task ReturnsExistingUser_WhenIdentityAlreadyResolved()
    {
        var identity = Identity();

        var first = await ResolveAsync(identity);
        var second = await ResolveAsync(identity);

        Assert.Equal(first.UserId, second.UserId);
        Assert.Equal(first.PublicId, second.PublicId);
        Assert.Equal(1, await CountUsersAsync());
    }

    [Fact]
    public async Task UpdatesUsernameAndEmail_WhenChanged()
    {
        var first = await ResolveAsync(Identity(username: "old", email: "old@example.com"));

        var updated = await ResolveAsync(Identity(username: "new", email: "new@example.com"));

        Assert.Equal(first.UserId, updated.UserId);
        var fromDb = await GetUserAsync("sub-1", "pocket-id");
        Assert.Equal("new", fromDb.Username);
        Assert.Equal("new@example.com", fromDb.Email);
        Assert.Equal(1, await CountUsersAsync());
    }

    [Fact]
    public async Task ResolvesToSingleUser_WhenCalledConcurrently()
    {
        var identity = Identity();

        var results = await Task.WhenAll(ResolveAsync(identity), ResolveAsync(identity));

        Assert.True(results[0].UserId > 0);
        Assert.Equal(results[0].UserId, results[1].UserId);
        Assert.Equal(1, await CountUsersAsync());
    }
}
