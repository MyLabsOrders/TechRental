using MediatR;
using Microsoft.EntityFrameworkCore;
using TechRental.Application.Common.Exceptions;
using TechRental.DataAccess.Abstractions;
using TechRental.Domain.Common.Exceptions;
using TechRental.Domain.Core.Abstractions;
using TechRental.Domain.Core.Orders;
using TechRental.Domain.Core.Users;
using static TechRental.Application.Contracts.Users.Commands.RemoveOrder;

namespace TechRental.Application.Handlers.Users;

internal class RemoveOrderHandler : IRequestHandler<Command>
{
    private readonly IDatabaseContext _context;

    public RemoveOrderHandler(IDatabaseContext context)
    {
        _context = context;
    }

    public async Task Handle(Command request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Id.Equals(request.UserId), cancellationToken);
        var order = await _context.Orders.FirstOrDefaultAsync(x => x.Id.Equals(request.OrderId), cancellationToken);

        if (user is null)
            throw EntityNotFoundException.For<User>(request.UserId);

        if (order is null)
            throw EntityNotFoundException.For<Order>(request.OrderId);

        ProcessOperation(order);
        user.RemoveOrder(order);

        await _context.SaveChangesAsync(cancellationToken);
    }

    private static void ProcessOperation(Order order)
    {
        order.OrderDate = null;
        order.Period = null;
        order.Amount = null;
        order.Status = OrderStatus.Available;
    }
}