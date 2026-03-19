using AosAdjutant.Api.Database;
using AosAdjutant.Api.Features.AttackProfiles;
using AosAdjutant.Api.Features.AttackProfiles.WeaponEffects;
using AosAdjutant.Api.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AosAdjutant.Api.Features.Units;

[Route("api/units/{unitId}/attack-profiles")]
[ApiController]
[Tags("Attack Profiles")]
public class UnitAttackProfileController(ApplicationDbContext context) : ControllerBase
{
    [HttpPost]
    [EndpointSummary("Create an attack profile under a unit")]
    [ProducesResponseType<AttackProfileResponseDto>(StatusCodes.Status201Created)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<AttackProfileResponseDto>> CreateAttackProfile(
        [FromRoute] int unitId,
        [FromBody] CreateAttackProfileDto attackProfileData
    )
    {
        var unitExists = await context.Units.AnyAsync(u => u.UnitId == unitId);
        if (!unitExists)
            return this.ApiProblem(new AppError(ErrorCode.NotFound, "Unit not found."));

        var isDuplicate =
            await context.AttackProfiles.AnyAsync(ap => ap.Name == attackProfileData.Name && ap.UnitId == unitId);
        if (isDuplicate)
            return this.ApiProblem(new AppError(ErrorCode.UniqueKeyError, "Attack profile already exists."));

        var newAttackProfileResult = AttackProfile.Create(
            attackProfileData.Name,
            attackProfileData.IsRanged,
            attackProfileData.Range,
            attackProfileData.Attacks,
            attackProfileData.ToHit,
            attackProfileData.ToWound,
            attackProfileData.Rend,
            attackProfileData.Damage,
            unitId
            //WeaponEffects = context.WeaponEffects.Where(we => attackProfileData.WeaponEffects.Contains(we.Key)).ToList()
        );

        if (!newAttackProfileResult.IsSuccess) return this.ApiProblem(newAttackProfileResult.GetError);

        var newAttackProfile = newAttackProfileResult.GetValue;
        // Because of race conditions this might still fail on UK/FK error
        // Ignore for now (won't occur in practice) but revisit in the future
        context.AttackProfiles.Add(newAttackProfile);
        await context.SaveChangesAsync();

        return Created(
            $"api/attack-profiles/{newAttackProfile.AttackProfileId}",
            new AttackProfileResponseDto(
                newAttackProfile.AttackProfileId,
                newAttackProfile.Name,
                newAttackProfile.IsRanged,
                newAttackProfile.Range,
                newAttackProfile.Attacks,
                newAttackProfile.ToHit,
                newAttackProfile.ToWound,
                newAttackProfile.Rend,
                newAttackProfile.Damage,
                newAttackProfile.UnitId,
                newAttackProfile.Version,
                newAttackProfile.WeaponEffects.Select(wp => new WeaponEffectResponseDto(wp.Key, wp.Name)).ToList()
            )
        );
    }

    [HttpGet]
    [EndpointSummary("Get all attack profiles for a unit")]
    [ProducesResponseType<List<AttackProfileResponseDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<List<AttackProfileResponseDto>>> GetAttackProfiles([FromRoute] int unitId)
    {
        var unitExists = await context.Units.AnyAsync(u => u.UnitId == unitId);
        if (!unitExists)
            return this.ApiProblem(new AppError(ErrorCode.NotFound, "Unit not found."));

        var attackProfiles = await context.AttackProfiles
            .AsNoTracking()
            .Where(ap => ap.UnitId == unitId)
            .Select(ap => new AttackProfileResponseDto(
                    ap.AttackProfileId,
                    ap.Name,
                    ap.IsRanged,
                    ap.Range,
                    ap.Attacks,
                    ap.ToHit,
                    ap.ToWound,
                    ap.Rend,
                    ap.Damage,
                    ap.UnitId,
                    ap.Version,
                    ap.WeaponEffects.Select(wp => new WeaponEffectResponseDto(wp.Key, wp.Name)).ToList()
                )
            )
            .ToListAsync();
        return Ok(attackProfiles);
    }
}
