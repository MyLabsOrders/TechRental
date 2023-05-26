namespace TechRental.Application.Common.Exceptions;

public class OrderAlreadyRentedException : ApplicationException
{
    public OrderAlreadyRentedException(Guid id) : base($"Order {id} already rented")
    {
    }

    public OrderAlreadyRentedException(string? message) : base(message)
    {
    }
}
