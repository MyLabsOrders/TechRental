using TechRental.Application.Dto.Orders;
using TechRental.Domain.Core.Orders;
using TechRental.Infrastructure.Mapping.Users;

namespace TechRental.Infrastructure.Mapping.Orders;

public static class OrderMapping
{
    public static IEnumerable<UserOrderDto> ToDto(this IEnumerable<Order> orders)
    {
        return orders?.Select(order => order.ToUserOrderDto()) ?? Array.Empty<UserOrderDto>();
    }

    public static OrderDto ToDto(this Order order)
    {
        return new OrderDto(
            order.Id,
            order.User.ToDto(),
            order.Name,
            order.Image.Value,
            order.Status.ToString(),
            order.Price,
            order.OrderDate,
            order.Amount,
            order.Period);
    }

    public static UserOrderDto ToUserOrderDto(this Order order)
    {
        return new UserOrderDto(
            order.Id,
            order.Status.ToString(),
            order.Name,
            order.Image.Value,
            order.Price,
            order.OrderDate,
            order.Amount,
            order.Period);
    }
}
