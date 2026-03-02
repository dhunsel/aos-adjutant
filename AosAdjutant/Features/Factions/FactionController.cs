using AosAdjutant.Shared;
using Microsoft.AspNetCore.Mvc;

namespace AosAdjutant.Features.Factions;

[Route("factions")]
[ApiController]
public class FactionController(FactionService factionService) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<FactionResponseDto>> CreateFaction([FromBody] CreateFactionDto factionData)
    {
        var factionResult = await factionService.CreateFaction(factionData);
        return factionResult.Match(Ok, this.ApiProblem);
    }

    [HttpGet]
    public async Task<ActionResult<List<FactionResponseDto>>> GetFactions()
    {
        var factions = await factionService.GetFactions();
        return Ok(factions);
    }

    [HttpGet("{factionId}")]
    public async Task<ActionResult<FactionResponseDto>> GetFaction([FromRoute] int factionId)
    {
        var factionResult = await factionService.GetFaction(factionId);
        return factionResult.Match(Ok, this.ApiProblem);
    }

    [HttpPut("{factionId}")]
    public async Task<ActionResult<FactionResponseDto>> UpdateFaction([FromRoute] int factionId,
        [FromBody] ChangeFactionDto factionData)
    {
        var factionResult = await factionService.UpdateFaction(factionId, factionData);
        return factionResult.Match(Ok, this.ApiProblem);
    }

    [HttpDelete("{factionId}")]
    public async Task<ActionResult> DeleteFaction([FromRoute] int factionId)
    {
        var deleteResult = await factionService.DeleteFaction(factionId);
        return deleteResult.Match(NoContent, this.ApiProblem);
    }

    [HttpGet("/throw")]
    public ActionResult Throw() => throw new Exception("Exception");
}