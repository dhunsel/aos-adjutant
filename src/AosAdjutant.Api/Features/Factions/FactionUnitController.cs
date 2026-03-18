using AosAdjutant.Api.Database;
using AosAdjutant.Api.Features.Units;
using AosAdjutant.Api.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AosAdjutant.Api.Features.Factions;

[Route("api/factions/{factionId}/units")]
[ApiController]
[Tags("Units")]
public class FactionUnitController(ApplicationDbContext context) : ControllerBase
{
    [HttpPost]
    [EndpointSummary("Create a unit under a faction")]
    [ProducesResponseType<UnitResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<UnitResponse>> CreateUnit(
        [FromRoute] int factionId,
        [FromBody] CreateUnitDto unitData
    )
    {
        var factionExists = await context.Factions.AnyAsync(f => f.FactionId == factionId);
        if (!factionExists)
            return this.ApiProblem(new AppError(ErrorCode.NotFound, "Faction not found."));

        var isDuplicate = await context.Units.AnyAsync(u => u.Name == unitData.Name && u.FactionId == factionId);
        if (isDuplicate)
            return this.ApiProblem(new AppError(ErrorCode.UniqueKeyError, "Unit already exists."));

        var newUnit = new Unit
        {
            Name = unitData.Name,
            Health = unitData.Health,
            Move = unitData.Move,
            Save = unitData.Save,
            Control = unitData.Control,
            WardSave = unitData.WardSave,
            FactionId = factionId,
        };

        // Because of race conditions this might still fail on UK/FK error
        // Ignore for now (won't occur in practice) but revisit in the future
        context.Units.Add(newUnit);
        await context.SaveChangesAsync();

        return Created(
            $"api/units/{newUnit.UnitId}",
            new UnitResponse(
                newUnit.UnitId,
                newUnit.Name,
                newUnit.Health,
                newUnit.Move,
                newUnit.Save,
                newUnit.Control,
                newUnit.WardSave,
                newUnit.FactionId,
                newUnit.Version
            )
        );
    }

    [HttpGet]
    [EndpointSummary("Get all units for a faction")]
    [ProducesResponseType<List<UnitResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<List<UnitResponse>>> GetUnits([FromRoute] int factionId)
    {
        var factionExists = await context.Factions.AnyAsync(f => f.FactionId == factionId);
        if (!factionExists)
            return this.ApiProblem(new AppError(ErrorCode.NotFound, "Faction not found."));

        var units = await context.Units
            .AsNoTracking()
            .Where(u => u.FactionId == factionId)
            .Select(u => new UnitResponse(
                    u.UnitId,
                    u.Name,
                    u.Health,
                    u.Move,
                    u.Save,
                    u.Control,
                    u.WardSave,
                    u.FactionId,
                    u.Version
                )
            )
            .ToListAsync();
        return Ok(units);
    }
}
