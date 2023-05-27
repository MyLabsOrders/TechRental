using MediatR;
using Microsoft.EntityFrameworkCore;
using TechRental.Application.Common.Exceptions;
using TechRental.DataAccess.Abstractions;
using TechRental.Domain.Common.Exceptions;
using TechRental.Domain.Core.Abstractions;
using TechRental.Domain.Core.Orders;
using TechRental.Domain.Core.Users;
using static TechRental.Application.Contracts.Users.Commands.AddOrder;

namespace TechRental.Application.Handlers.Users;

internal class AddOrderHandler : IRequestHandler<Command>
{
    private readonly IDatabaseContext _context;

    public AddOrderHandler(IDatabaseContext context)
    {
        _context = context;
    }

    public async Task Handle(Command request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Id.Equals(request.UserId), cancellationToken);

        if (user is null)
            throw EntityNotFoundException.For<User>(request.UserId);

        var absent = request.Orders.ExceptBy(_context.Orders.Select(order => order.Id), dto => dto.OrderId);
        if (absent.Any())
            throw new EntityNotFoundException($"Orders with ids {string.Join(", ", absent.Select(dto => dto.OrderId))} were not found.");

        var orders = (await _context.Orders.ToListAsync(cancellationToken)).Join(
            request.Orders,
            order => order.Id,
            dto => dto.OrderId,
            (order, dto) => order).ToList();


        var rented = orders.Where(order => order.Status == OrderStatus.Rented);
        if (rented.Any())
            throw new OrderAlreadyRentedException($"Orders with ids {string.Join(", ", rented.Select(order => order.Id))} are already rented.");

        ProcessTransaction(user, orders, request);

        await _context.SaveChangesAsync(cancellationToken);
    }

    private static void ProcessTransaction(User user, IEnumerable<Order> orders, Command request)
    {
        user.Money -= orders.Select(order => order.TotalPrice).Sum();
        var orderDate = DateTime.UtcNow;
        foreach (var (order, dto) in orders.Zip(request.Orders))
        {
            order.OrderDate = orderDate;
            order.Amount = dto.Amount;
            order.Period = dto.Days;
            order.Status = OrderStatus.Rented;
            user.AddOrder(order);
        }
    }
}
