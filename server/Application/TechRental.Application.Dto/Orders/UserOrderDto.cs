namespace TechRental.Application.Dto.Orders;

public record UserOrderDto(
    Guid Id,
    string Status,
    string Name,
    string Company,
    string Image,
    decimal Total,
    DateTime? OrderDate,
    int? Count,
    int? Days);
