using MediatR;
using TechRental.Application.Dto.Orders;

namespace TechRental.Application.Contracts.Orders.Queries;

internal static class GetOrder
{
    public record Query(DateTime OrderTime) : IRequest<Response>;

    public record Response(IList<OrderDto> Orders);
}
