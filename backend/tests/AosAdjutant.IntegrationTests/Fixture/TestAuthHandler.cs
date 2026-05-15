using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AosAdjutant.IntegrationTests.Fixture;

public class TestAuthHandler(
    IOptionsMonitor<AuthenticationSchemeOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder
) : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
{
    public const string SchemeName = "Test";

    /// <summary>
    /// Per-request header letting a test pick the principal. Absent or "admin"
    /// keeps the original always-admin behaviour, so existing tests are
    /// unaffected. "user" is authenticated but not in "admins"; "anonymous"
    /// (or any unknown value) is unauthenticated.
    /// </summary>
    public const string AuthHeader = "X-Test-Auth";

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var mode = Request.Headers[AuthHeader].ToString();

        if (string.IsNullOrEmpty(mode) || mode.Equals("admin", StringComparison.OrdinalIgnoreCase))
            return Task.FromResult(Authenticate(isAdmin: true));

        if (mode.Equals("user", StringComparison.OrdinalIgnoreCase))
            return Task.FromResult(Authenticate(isAdmin: false));

        // "anonymous" and any unknown value: no authenticated principal.
        return Task.FromResult(AuthenticateResult.NoResult());
    }

    private AuthenticateResult Authenticate(bool isAdmin)
    {
        var claims = new List<Claim> { new("preferred_username", "integration-test") };
        if (isAdmin)
            claims.Add(new Claim("groups", "admins"));

        var identity = new ClaimsIdentity(
            claims,
            authenticationType: SchemeName,
            nameType: "preferred_username",
            roleType: "groups"
        );
        var ticket = new AuthenticationTicket(new ClaimsPrincipal(identity), SchemeName);

        return AuthenticateResult.Success(ticket);
    }
}
