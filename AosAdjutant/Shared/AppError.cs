namespace AosAdjutant.Shared;

public enum ErrorCode
{
    NotFound,
    ValidationError,
    ConcurrencyError,
    UniqueKeyError
}

public class AppError(ErrorCode code, string message)
{
    public ErrorCode Code { get; } = code;
    public string Message { get; } = message;
}