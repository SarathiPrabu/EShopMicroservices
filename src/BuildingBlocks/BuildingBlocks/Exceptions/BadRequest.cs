namespace BuildingBlocks.Exceptions;

public class BadRequest : Exception
{
    public BadRequest(string message) : base(message)
    {
    }

    public BadRequest(string message, string details) : base(message)
    {
        Details = details;
    }
    private string? Details { get; }
}