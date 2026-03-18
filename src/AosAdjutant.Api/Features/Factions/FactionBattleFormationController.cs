using AosAdjutant.Api.Database;
using AosAdjutant.Api.Features.BattleFormations;
using AosAdjutant.Api.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AosAdjutant.Api.Features.Factions;

[Route("api/factions/{factionId}/battle-formations")]
[ApiController]
[Tags("Battle Formations")]
public class FactionBattleFormationController(ApplicationDbContext context) : ControllerBase
{
    [HttpPost]
    [EndpointSummary("Create a battle formation under a faction")]
    [ProducesResponseType<BattleFormationResponseDto>(StatusCodes.Status201Created)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<BattleFormationResponseDto>> CreateBattleFormation(
        [FromRoute] int factionId,
        [FromBody] CreateBattleFormationDto battleFormationData
    )
    {
        var factionExists = await context.Factions.AnyAsync(f => f.FactionId == factionId);
        if (!factionExists)
            return this.ApiProblem(new AppError(ErrorCode.NotFound, "Faction not found."));

        var isDuplicate = await context.BattleFormations.AnyAsync(bf =>
            bf.Name == battleFormationData.Name && bf.FactionId == factionId
        );
        if (isDuplicate)
            return this.ApiProblem(new AppError(ErrorCode.UniqueKeyError, "Battle formation already exists."));

        var newBattleFormation = new BattleFormation { Name = battleFormationData.Name, FactionId = factionId, };

        // Because of race conditions this might still fail on UK/FK error
        // Ignore for now (won't occur in practice) but revisit in the future
        context.BattleFormations.Add(newBattleFormation);
        await context.SaveChangesAsync();

        return Created(
            $"api/battle-formations/{newBattleFormation.BattleFormationId}",
            new BattleFormationResponseDto(
                newBattleFormation.BattleFormationId,
                newBattleFormation.Name,
                newBattleFormation.FactionId,
                newBattleFormation.Version
            )
        );
    }

    [HttpGet]
    [EndpointSummary("Get all battle formations for a faction")]
    [ProducesResponseType<List<BattleFormationResponseDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<List<BattleFormationResponseDto>>> GetBattleFormations([FromRoute] int factionId)
    {
        var factionExists = await context.Factions.AnyAsync(f => f.FactionId == factionId);
        if (!factionExists)
            return this.ApiProblem(new AppError(ErrorCode.NotFound, "Faction not found."));

        var battleFormations = await context.BattleFormations
            .AsNoTracking()
            .Where(bf => bf.FactionId == factionId)
            .Select(bf => new BattleFormationResponseDto(bf.BattleFormationId, bf.Name, bf.FactionId, bf.Version))
            .ToListAsync();
        return Ok(battleFormations);
    }
}
