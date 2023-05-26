namespace TechRental.Presentation.Contracts.Orders;

public record CreateOrderRequest(string Name, string? OrderImage, string Status, decimal Price, int Amount);
