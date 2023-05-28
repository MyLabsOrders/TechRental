using MediatR;
using TechRental.Application.Abstractions.Identity;
using TechRental.Application.Common.Exceptions;
using TechRental.DataAccess.Abstractions;
using TechRental.Domain.Core.Abstractions;
using TechRental.Domain.Core.Orders;
using TechRental.Domain.Core.Users;
using TechRental.Infrastructure.Mapping.Orders;
using static TechRental.Application.Contracts.Orders.Commands.CreateOrder;

namespace TechRental.Application.Handlers.Orders;

internal class CreateOrderHandler : IRequestHandler<Command, Response>
{
    private readonly IDatabaseContext _context;
    private readonly ICurrentUser _currentUser;

    public CreateOrderHandler(IDatabaseContext context, ICurrentUser currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
    {
        if (_currentUser.CanManageOrders() is false)
            throw AccessDeniedException.AccessViolation();

        var order = new Order(Guid.NewGuid(),
                              user: null,
                              name: request.Name,
                              company: request.Company,
                              image: new Image(request.OrderImage),
                              status: Enum.Parse<OrderStatus>(request.Status, true),
                              price: request.Price,
                              orderDate: null);

        _context.Orders.Add(order);
        await _context.SaveChangesAsync(cancellationToken);

        return new Response(order.ToDto());
    }
}
