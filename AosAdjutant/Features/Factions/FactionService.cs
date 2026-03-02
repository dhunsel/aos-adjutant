using AosAdjutant.Database;
using AosAdjutant.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AosAdjutant.Features.Factions;

public class FactionService(ApplicationDbContext context, ILogger<FactionService> logger)
{
    public async Task<Result<FactionResponseDto>> CreateFaction(CreateFactionDto factionData)
    {
        var newFaction = new Faction
        {
            Name = factionData.Name
        };

        // First check to catch duplicates. Race conditions could still occur, these are caught when calling 
        // TrySaveChangesAsync below
        if (await ExistsDuplicateFaction(newFaction))
        {
            logger.LogWarning("Faction creation rejected: name '{Name}' already exists", factionData.Name);
            return Result<FactionResponseDto>.Failure(new AppError(ErrorCode.UniqueKeyError,
                "Faction already exists."));
        }

        context.Factions.Add(newFaction);

        return await context.TrySaveChangesAsync(
            () => newFaction.ToResponseDto(),
            logger,
            uniqueViolationMessage: "Faction already exists.");
    }

    public async Task<List<FactionResponseDto>> GetFactions()
    {
        return await context.Factions.AsNoTracking().Select(f => f.ToResponseDto()).ToListAsync();
    }

    public async Task<Result<FactionResponseDto>> GetFaction(int factionId)
    {
        var faction = await context.Factions.AsNoTracking().FirstOrDefaultAsync(f => f.FactionId == factionId);
        return faction is null
            ? Result<FactionResponseDto>.Failure(new AppError(ErrorCode.NotFound, "Faction not found."))
            : Result<FactionResponseDto>.Success(faction.ToResponseDto());
    }

    public async Task<Result<FactionResponseDto>> UpdateFaction(int factionId, ChangeFactionDto factionData)
    {
        var faction = await context.Factions.FirstOrDefaultAsync(f => f.FactionId == factionId);

        if (faction is null)
        {
            logger.LogWarning("Update failed: faction {FactionId} not found", factionId);
            return Result<FactionResponseDto>.Failure(new AppError(ErrorCode.NotFound, "Faction not found."));
        }

        context.Entry(faction).Property(f => f.Version).OriginalValue = factionData.Version;

        faction.Name = factionData.Name;

        return await context.TrySaveChangesAsync(
            () => faction.ToResponseDto(),
            logger,
            concurrencyMessage: "Faction was already modified in the background.",
            uniqueViolationMessage: "Faction already exists.");
    }

    public async Task<Result> DeleteFaction(int factionId)
    {
        var faction = await context.Factions.FirstOrDefaultAsync(f => f.FactionId == factionId);
        if (faction is null)
        {
            logger.LogWarning("Delete failed: faction {FactionId} not found", factionId);
            return Result.Failure(new AppError(ErrorCode.NotFound, "Faction not found."));
        }

        context.Factions.Remove(faction);

        return await context.TrySaveChangesAsync(logger);
    }

    private async Task<bool> ExistsDuplicateFaction(Faction faction)
    {
        var duplicate = await context.Factions.FirstOrDefaultAsync(f => f.Name == faction.Name);
        return duplicate is not null;
    }
}