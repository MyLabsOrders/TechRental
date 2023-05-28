using MediatR;
using TechRental.Application.Dto.Orders;

namespace TechRental.Application.Contracts.Orders.Queries;

internal static class GetStats
{
    public record Query(DateTime From, DateTime To) : IRequest<Response>;

    public record Response(Stream Stream);
}
