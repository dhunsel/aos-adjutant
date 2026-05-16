using System.Net;
using System.Net.Http.Json;
using AosAdjutant.Api.Features.BattleFormations;
using AosAdjutant.Api.Features.Factions;
using AosAdjutant.IntegrationTests.Fixture;

namespace AosAdjutant.IntegrationTests.Features.BattleFormations;

public class BattleFormationEndpointTests(ApiFactory factory) : EndpointTestsBase(factory)
{
    private async Task<BattleFormationResponseDto> CreateBattleFormationAsync()
    {
        var factionResponse = await Client.PostAsJsonAsync(
            "/api/factions",
            new CreateFactionDto { Name = "TestFaction", GrandAlliance = GrandAlliance.Order },
            JsonOptions
        );
        var faction = (
            await factionResponse.Content.ReadFromJsonAsync<FactionResponseDto>(JsonOptions)
        )!;

        var response = await Client.PostAsJsonAsync(
            $"/api/factions/{faction.FactionId}/battle-formations",
            new CreateBattleFormationDto { Name = "TestBattleFormation" }
        );
        return (await response.Content.ReadFromJsonAsync<BattleFormationResponseDto>(JsonOptions))!;
    }

    // --- GET /api/battle-formations/{id} ---

    [Fact]
    public async Task GetBattleFormation_Returns200()
    {
        var created = await CreateBattleFormationAsync();

        var response = await Client.GetAsync($"/api/battle-formations/{created.BattleFormationId}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<BattleFormationResponseDto>(
            JsonOptions
        );
        Assert.NotNull(body);
        Assert.Equal("TestBattleFormation", body.Name);
    }

    // --- PUT /api/battle-formations/{id} ---

    [Fact]
    public async Task UpdateBattleFormation_Returns200()
    {
        var created = await CreateBattleFormationAsync();

        var response = await Client.PutAsJsonAsync(
            $"/api/battle-formations/{created.BattleFormationId}",
            new ChangeBattleFormationDto
            {
                Name = "UpdatedBattleFormation",
                Version = created.Version,
            }
        );

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<BattleFormationResponseDto>(
            JsonOptions
        );
        Assert.NotNull(body);
        Assert.Equal("UpdatedBattleFormation", body.Name);
    }

    [Fact]
    public async Task UpdateBattleFormation_Returns400_WhenNameIsEmpty()
    {
        var created = await CreateBattleFormationAsync();

        var response = await Client.PutAsJsonAsync(
            $"/api/battle-formations/{created.BattleFormationId}",
            new ChangeBattleFormationDto { Name = "", Version = created.Version }
        );

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task UpdateBattleFormation_Returns409_WhenNameTaken()
    {
        var existing = await CreateBattleFormationAsync();
        var secondResponse = await Client.PostAsJsonAsync(
            $"/api/factions/{existing.FactionId}/battle-formations",
            new CreateBattleFormationDto { Name = "SecondFormation" }
        );
        var target = (
            await secondResponse.Content.ReadFromJsonAsync<BattleFormationResponseDto>(JsonOptions)
        )!;

        var response = await Client.PutAsJsonAsync(
            $"/api/battle-formations/{target.BattleFormationId}",
            new ChangeBattleFormationDto { Name = existing.Name, Version = target.Version }
        );

        await AssertProblem(response, HttpStatusCode.Conflict, "UniqueKeyError");
    }

    [Fact]
    public async Task UpdateBattleFormation_Returns409_WhenVersionMismatch()
    {
        var created = await CreateBattleFormationAsync();

        var response = await Client.PutAsJsonAsync(
            $"/api/battle-formations/{created.BattleFormationId}",
            new ChangeBattleFormationDto
            {
                Name = "UpdatedBattleFormation",
                Version = created.Version + 1u,
            }
        );

        await AssertProblem(response, HttpStatusCode.Conflict, "ConcurrencyError");
    }

    // --- DELETE /api/battle-formations/{id} ---

    [Fact]
    public async Task DeleteBattleFormation_Returns204()
    {
        var created = await CreateBattleFormationAsync();

        var response = await Client.DeleteAsync(
            $"/api/battle-formations/{created.BattleFormationId}"
        );

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    // --- Not-found ---

    [Fact]
    public Task UpdateBattleFormation_Returns404_WhenMissing() =>
        AssertRequestNotFound(
            HttpMethod.Put,
            "/api/battle-formations/999",
            new ChangeBattleFormationDto { Name = "UpdatedBattleFormation", Version = 0 }
        );
}
