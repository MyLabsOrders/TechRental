namespace TechRental.Application.Common.Exceptions;

public class InvalidGenderException : ApplicationException
{
    public InvalidGenderException() : base("Invalid gender")
    {
    }

    public InvalidGenderException(string? message) : base(message)
    {
    }
}
