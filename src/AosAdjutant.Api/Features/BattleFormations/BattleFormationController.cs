using AosAdjutant.Api.Database;
using AosAdjutant.Api.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AosAdjutant.Api.Features.BattleFormations;

[Route("api/battle-formations")]
[ApiController]
public class BattleFormationController(ApplicationDbContext context) : ControllerBase
{
    [HttpGet("{battleFormationId}")]
    public async Task<ActionResult<BattleFormationResponseDto>> GetBattleFormation([FromRoute] int battleFormationId)
    {
        var battleFormation = await context.BattleFormations
            .AsNoTracking()
            .FirstOrDefaultAsync(bf => bf.BattleFormationId == battleFormationId);

        return battleFormation is null
            ? this.ApiProblem(new AppError(ErrorCode.NotFound, "Battle formation not found."))
            : Ok(
                new BattleFormationResponseDto(
                    battleFormation.BattleFormationId,
                    battleFormation.Name,
                    battleFormation.FactionId,
                    battleFormation.Version
                )
            );
    }

    [HttpPut("{battleFormationId}")]
    public async Task<ActionResult<BattleFormationResponseDto>> UpdateBattleFormation(
        [FromRoute] int battleFormationId,
        [FromBody] ChangeBattleFormationDto battleFormationData
    )
    {
        var battleFormation = await context.BattleFormations.FindAsync(battleFormationId);

        if (battleFormation is null)
            return this.ApiProblem(new AppError(ErrorCode.NotFound, "Battle formation not found."));

        if (battleFormation.Version != battleFormationData.Version)
            return this.ApiProblem(
                new AppError(ErrorCode.ConcurrencyError, "Battle formation was already modified in the background.")
            );

        var isDuplicate = await context.BattleFormations.AnyAsync(bf =>
            bf.Name == battleFormationData.Name && bf.FactionId == battleFormation.FactionId &&
            bf.BattleFormationId != battleFormationId
        );
        if (isDuplicate)
            return this.ApiProblem(new AppError(ErrorCode.UniqueKeyError, "Battle formation already exists."));

        battleFormation.Name = battleFormationData.Name;
        await context.SaveChangesAsync();

        return Ok(
            new BattleFormationResponseDto(
                battleFormation.BattleFormationId,
                battleFormation.Name,
                battleFormation.FactionId,
                battleFormation.Version
            )
        );
    }

    [HttpDelete("{battleFormationId}")]
    public async Task<ActionResult> DeleteBattleFormation([FromRoute] int battleFormationId)
    {
        var battleFormation = await context.BattleFormations.FindAsync(battleFormationId);

        if (battleFormation is null)
            return this.ApiProblem(new AppError(ErrorCode.NotFound, "Battle formation not found."));

        context.BattleFormations.Remove(battleFormation);
        await context.SaveChangesAsync();

        return NoContent();
    }
}
