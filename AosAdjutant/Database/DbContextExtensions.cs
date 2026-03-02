using AosAdjutant.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace AosAdjutant.Database;

public static class DbContextExtensions
{
    extension(DbContext context)
    {
        public async Task<Result<T>> TrySaveChangesAsync<T>(Func<T> getValue,
            ILogger? logger = null,
            string concurrencyMessage = "The resource was modified concurrently.",
            string uniqueViolationMessage = "A duplicate value already exists.")
        {
            try
            {
                await context.SaveChangesAsync();
                return Result<T>.Success(getValue());
            }
            catch (DbUpdateConcurrencyException ex)
            {
                logger?.LogWarning(ex, "Concurrency conflict during SaveChanges");
                return Result<T>.Failure(new AppError(ErrorCode.ConcurrencyError, concurrencyMessage));
            }
            catch (DbUpdateException ex) when (ex.InnerException is PostgresException
                                               {
                                                   SqlState: PostgresErrorCodes.UniqueViolation
                                               })
            {
                logger?.LogWarning(ex, "Unique constraint violation during SaveChanges");
                return Result<T>.Failure(new AppError(ErrorCode.UniqueKeyError, uniqueViolationMessage));
            }
        }

        public async Task<Result> TrySaveChangesAsync(
            ILogger? logger = null,
            string concurrencyMessage = "The resource was modified concurrently.",
            string uniqueViolationMessage = "A duplicate value already exists.")
        {
            try
            {
                await context.SaveChangesAsync();
                return Result.Success();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                logger?.LogWarning(ex, "Concurrency conflict during SaveChanges");
                return Result.Failure(new AppError(ErrorCode.ConcurrencyError, concurrencyMessage));
            }
            catch (DbUpdateException ex) when (ex.InnerException is PostgresException
                                               {
                                                   SqlState: PostgresErrorCodes.UniqueViolation
                                               })
            {
                logger?.LogWarning(ex, "Unique constraint violation during SaveChanges");
                return Result.Failure(new AppError(ErrorCode.UniqueKeyError, uniqueViolationMessage));
            }
        }
    }
}