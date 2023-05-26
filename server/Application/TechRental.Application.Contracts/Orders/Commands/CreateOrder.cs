using MediatR;
using TechRental.Application.Dto.Orders;

namespace TechRental.Application.Contracts.Orders.Commands;

internal static class CreateOrder
{
    public record Command(string Name, string Company, string OrderImage, string Status, decimal Price) : IRequest<Response>;

    public record Response(OrderDto Order);
}
