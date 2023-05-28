using MediatR;
using Microsoft.EntityFrameworkCore;
using TechRental.Application.Common.Exceptions;
using TechRental.DataAccess.Abstractions;
using TechRental.Domain.Common.Exceptions;
using TechRental.Domain.Core.Orders;
using TechRental.Infrastructure.Mapping.Orders;
using static TechRental.Application.Contracts.Orders.Queries.GetOrder;

namespace TechRental.Application.Handlers.Orders;

internal class GetOrderHandler : IRequestHandler<Query, Response>
{
    private readonly IDatabaseContext _context;

    public GetOrderHandler(IDatabaseContext context)
    {
        _context = context;
    }

    public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
    {
        var orders = await _context.Orders
            .Where(order => order.OrderDate == request.OrderTime)
            .Include(order => order.User)
            .ToListAsync(cancellationToken);

        if (!orders.Any())
            throw new EntityNotFoundException("Order with this date does not exist");

        return new Response(orders.Select(order => order.ToDto()).ToList());
    }
}
