using AosAdjutant.IntegrationTests.Fixture;

namespace AosAdjutant.IntegrationTests;

/// <summary>
/// Cross-cutting contract: every resource route returns a 404 NotFound problem
/// for an id (or parent id) that does not exist. Covers the body-free verbs
/// (GET/DELETE single, GET nested list); write verbs that need a valid DTO are
/// covered as one-liners in their own endpoint test files.
/// </summary>
public class NotFoundContractTests(ApiFactory factory) : EndpointTestsBase(factory)
{
    public static TheoryData<string, string> MissingRoutes =>
        new()
        {
            { "GET", "/api/abilities/999" },
            { "DELETE", "/api/abilities/999" },
            { "GET", "/api/factions/999" },
            { "DELETE", "/api/factions/999" },
            { "GET", "/api/units/999" },
            { "DELETE", "/api/units/999" },
            { "GET", "/api/attack-profiles/999" },
            { "DELETE", "/api/attack-profiles/999" },
            { "GET", "/api/battle-formations/999" },
            { "DELETE", "/api/battle-formations/999" },
            { "GET", "/api/factions/999/units" },
            { "GET", "/api/factions/999/abilities" },
            { "GET", "/api/factions/999/battle-formations" },
            { "GET", "/api/units/999/abilities" },
            { "GET", "/api/units/999/attack-profiles" },
            { "GET", "/api/battle-formations/999/abilities" },
        };

    [Theory]
    [MemberData(nameof(MissingRoutes))]
    public Task Returns404_WhenResourceMissing(string method, string url) =>
        AssertRequestNotFound(new HttpMethod(method), url);
}
