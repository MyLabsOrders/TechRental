using MediatR;
using TechRental.Application.Abstractions.Identity;
using TechRental.Application.Common.Exceptions;
using TechRental.DataAccess.Abstractions;
using TechRental.Domain.Common.Exceptions;
using TechRental.Domain.Core.Abstractions;
using TechRental.Domain.Core.Orders;
using static TechRental.Application.Contracts.Orders.Commands.ChangeOrderStatus;

namespace TechRental.Application.Handlers.Orders;

internal class ChangeOrderStatusHandler : IRequestHandler<Command>
{
    private readonly IDatabaseContext _context;
    private readonly ICurrentUser _currentUser;

    public ChangeOrderStatusHandler(IDatabaseContext context, ICurrentUser currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task Handle(Command request, CancellationToken cancellationToken)
    {
        if (_currentUser.CanManageOrders() is false)
            throw new UnauthorizedException("You are not authorized");

        var order = await _context.Orders.FindAsync(new object?[] { request.OrderId }, cancellationToken: cancellationToken);

        if (order is null)
            throw EntityNotFoundException.For<Order>(request.OrderId);

        order.Status = Enum.Parse<OrderStatus>(request.Status);

        await _context.SaveChangesAsync(cancellationToken);
    }
}
