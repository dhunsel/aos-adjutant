using AosAdjutant.Api.Database;
using AosAdjutant.Api.Shared;
using Microsoft.EntityFrameworkCore;

namespace AosAdjutant.Api.Features.AttackProfiles;

public class AttackProfileService(ApplicationDbContext context)
{
    public async Task<Result<AttackProfile>> CreateAttackProfile(int unitId, CreateAttackProfileDto attackProfileData)
    {
        var unitExists = await context.Units.AnyAsync(u => u.UnitId == unitId);
        if (!unitExists)
            return Result<AttackProfile>.Failure(new AppError(ErrorCode.NotFound, "Unit not found."));

        var isDuplicate =
            await context.AttackProfiles.AnyAsync(ap => ap.Name == attackProfileData.Name && ap.UnitId == unitId);
        if (isDuplicate)
            return Result<AttackProfile>.Failure(
                new AppError(ErrorCode.UniqueKeyError, "Attack profile already exists.")
            );

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

        if (!newAttackProfileResult.IsSuccess) return newAttackProfileResult;

        var newAttackProfile = newAttackProfileResult.GetValue;
        // Because of race conditions this might still fail on UK/FK error
        // Ignore for now (won't occur in practice) but revisit in the future
        context.AttackProfiles.Add(newAttackProfile);
        await context.SaveChangesAsync();

        return Result<AttackProfile>.Success(newAttackProfile);
    }

    public async Task<Result<List<AttackProfile>>> GetUnitAttackProfiles(int unitId)
    {
        var unitExists = await context.Units.AnyAsync(u => u.UnitId == unitId);
        if (!unitExists)
            return Result<List<AttackProfile>>.Failure(new AppError(ErrorCode.NotFound, "Unit not found."));

        var attackProfiles = await context.AttackProfiles.AsNoTracking().Where(ap => ap.UnitId == unitId).ToListAsync();
        return Result<List<AttackProfile>>.Success(attackProfiles);
    }

    public async Task<Result<AttackProfile>> GetAttackProfile(int attackProfileId)
    {
        var attackProfile = await context.AttackProfiles
            .AsNoTracking()
            .Include(ap => ap.WeaponEffects)
            .FirstOrDefaultAsync(ap => ap.AttackProfileId == attackProfileId);

        return attackProfile is null
            ? Result<AttackProfile>.Failure(new AppError(ErrorCode.NotFound, "Attack profile not found."))
            : Result<AttackProfile>.Success(attackProfile);
    }

    public async Task<Result<AttackProfile>> UpdateAttackProfile(
        int attackProfileId,
        ChangeAttackProfileDto attackProfileData
    )
    {
        // Both Create and Update should perform validations but are part of different classes.
        // -> Reason to introduce service class which both controller classes call?
        var attackProfile = await context.AttackProfiles
            .Include(ap => ap.WeaponEffects)
            .FirstOrDefaultAsync(ap => ap.AttackProfileId == attackProfileId);

        if (attackProfile is null)
            return Result<AttackProfile>.Failure(new AppError(ErrorCode.NotFound, "Attack profile not found."));

        if (attackProfile.Version != attackProfileData.Version)
            return Result<AttackProfile>.Failure(
                new AppError(ErrorCode.ConcurrencyError, "Attack profile was already modified in the background.")
            );

        var isDuplicate = await context.AttackProfiles.AnyAsync(ap =>
            ap.Name == attackProfileData.Name && ap.UnitId == attackProfile.UnitId &&
            ap.AttackProfileId != attackProfileId
        );
        if (isDuplicate)
            return Result<AttackProfile>.Failure(
                new AppError(ErrorCode.UniqueKeyError, "Attack profile already exists.")
            );

        var changeResult = attackProfile.Change(
            attackProfileData.Name,
            attackProfileData.IsRanged,
            attackProfileData.Range,
            attackProfileData.Attacks,
            attackProfileData.ToHit,
            attackProfileData.ToWound,
            attackProfileData.Rend,
            attackProfileData.Damage
            //WeaponEffects = context.WeaponEffects.Where(we => attackProfileData.WeaponEffects.Contains(we.Key)).ToList()
        );

        if (!changeResult.IsSuccess) return Result<AttackProfile>.Failure(changeResult.GetError);

        //attackProfile.WeaponEffects.Clear();
        //foreach (var we in (await context.WeaponEffects
        //             .Where(we => attackProfileData.WeaponEffects.Contains(we.Key))
        //             .ToListAsync()))
        //    attackProfile.WeaponEffects.Add(we);

        await context.SaveChangesAsync();

        return Result<AttackProfile>.Success(attackProfile);
    }

    public async Task<Result> DeleteAttackProfile(int attackProfileId)
    {
        var attackProfile = await context.AttackProfiles.FindAsync(attackProfileId);

        if (attackProfile is null)
            return Result.Failure(new AppError(ErrorCode.NotFound, "Attack profile not found."));

        context.AttackProfiles.Remove(attackProfile);
        await context.SaveChangesAsync();

        return Result.Success();
    }
}
