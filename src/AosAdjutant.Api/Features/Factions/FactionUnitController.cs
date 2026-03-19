using AosAdjutant.Api.Features.Units;
using AosAdjutant.Api.Shared;
using Microsoft.AspNetCore.Mvc;

namespace AosAdjutant.Api.Features.Factions;

[Route("api/factions/{factionId}/units")]
[ApiController]
[Tags("Units")]
public class FactionUnitController(UnitService unitService) : ControllerBase
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
        var unitResult = await unitService.CreateUnit(factionId, unitData);
        return unitResult.Match(
            u => Created(
                $"api/units/{u.UnitId}",
                new UnitResponse(
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
            ),
            this.ApiProblem
        );
    }

    [HttpGet]
    [EndpointSummary("Get all units for a faction")]
    [ProducesResponseType<List<UnitResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<List<UnitResponse>>> GetUnits([FromRoute] int factionId)
    {
        var unitsResult = await unitService.GetFactionUnits(factionId);
        return unitsResult.Match(
            units => Ok(
                units.Select(u => new UnitResponse(
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
            ),
            this.ApiProblem
        );
    }
}
