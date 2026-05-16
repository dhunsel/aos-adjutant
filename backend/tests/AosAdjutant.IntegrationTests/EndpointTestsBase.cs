using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using AosAdjutant.Api.Database;
using AosAdjutant.IntegrationTests.Fixture;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AosAdjutant.IntegrationTests;

[Collection(nameof(ApiCollection))]
public class EndpointTestsBase(ApiFactory factory) : IAsyncLifetime
{
    protected readonly HttpClient Client = factory.CreateClient();

    protected static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) },
    };

    public async Task InitializeAsync()
    {
        using var scope = factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var tableNames = context
            .Model.GetEntityTypes()
            .Where(e => e.GetTableName() != "weapon_effect") // Weapon effect contains seed data
            .Select(e => e.GetTableName())
            .Distinct();

#pragma warning disable EF1002
        await context.Database.ExecuteSqlRawAsync(
            $"TRUNCATE TABLE {string.Join(", ", tableNames.Select(t => $"\"{t}\""))} CASCADE"
        );
#pragma warning restore EF1002
    }

    public Task DisposeAsync() => Task.CompletedTask;

    protected static async Task AssertProblem(
        HttpResponseMessage response,
        HttpStatusCode status,
        string title
    )
    {
        Assert.Equal(status, response.StatusCode);
        var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>(JsonOptions);
        Assert.NotNull(problem);
        Assert.Equal(title, problem.Title);
    }

    protected async Task AssertRequestNotFound(HttpMethod method, string url, object? body = null)
    {
        using var request = new HttpRequestMessage(method, url);
        if (body is not null)
            request.Content = JsonContent.Create(body, options: JsonOptions);
        var response = await Client.SendAsync(request);
        await AssertProblem(response, HttpStatusCode.NotFound, "NotFound");
    }
}
