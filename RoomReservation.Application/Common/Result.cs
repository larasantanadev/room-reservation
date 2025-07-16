namespace RoomReservation.Application.Common;

public class Result<T>
{
    public bool Success { get; set; }
    public List<string> Errors { get; set; } = new();
    public T? Data { get; set; }

    public static Result<T> Ok(T data) => new()
    {
        Success = true,
        Data = data
    };

    public static Result<T> Fail(string error) => new()
    {
        Success = false,
        Errors = new List<string> { error }
    };

    public static Result<T> Fail(List<string> errors) => new()
    {
        Success = false,
        Errors = errors
    };
}
