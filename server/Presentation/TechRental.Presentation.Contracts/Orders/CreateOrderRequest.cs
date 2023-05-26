namespace TechRental.Presentation.Contracts.Orders;

public record CreateOrderRequest(
    string Name,
    string Company,
    string? OrderImage,
    string Status,
    decimal Price);
