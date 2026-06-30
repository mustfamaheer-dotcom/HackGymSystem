namespace Gym.Shared.Common;

public class Result<T>
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public T? Data { get; }
    public string? Message { get; }
    public string[]? Errors { get; }

    private Result(bool isSuccess, T? data, string? message, string[]? errors)
    {
        IsSuccess = isSuccess;
        Data = data;
        Message = message;
        Errors = errors;
    }

    public static Result<T> Success(T data, string? message = null) =>
        new(true, data, message, null);

    public static Result<T> Failure(string message, params string[] errors) =>
        new(false, default, message, errors);

    public static Result<T> Failure(string[] errors) =>
        new(false, default, null, errors);
}

public class Result
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public string? Message { get; }
    public string[]? Errors { get; }

    private Result(bool isSuccess, string? message, string[]? errors)
    {
        IsSuccess = isSuccess;
        Message = message;
        Errors = errors;
    }

    public static Result Success(string? message = null) =>
        new(true, message, null);

    public static Result Failure(string message, params string[] errors) =>
        new(false, message, errors);
}
