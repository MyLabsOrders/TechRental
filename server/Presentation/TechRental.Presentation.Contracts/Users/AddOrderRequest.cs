namespace TechRental.Presentation.Contracts.Users;

public record AddOrderRequest(Guid OrderId, int Count, int Days);
