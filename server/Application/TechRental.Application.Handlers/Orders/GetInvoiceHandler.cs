using System.Text;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TechRental.Application.Abstractions.Identity;
using TechRental.Application.Common.Exceptions;
using TechRental.DataAccess.Abstractions;
using TechRental.Domain.Common.Exceptions;
using TechRental.Domain.Core.Orders;
using TechRental.Domain.Core.Users;
using TechRental.Infrastructure.Mapping.Orders;
using static TechRental.Application.Contracts.Orders.Queries.GetInvoice;

namespace TechRental.Application.Handlers.Orders;

internal class GetInvoiceHandler : IRequestHandler<Query, Response>
{
    private readonly IDatabaseContext _context;
    private readonly ICurrentUser _currentUser;
    private static int _id = 1;

    public GetInvoiceHandler(IDatabaseContext context, ICurrentUser currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(
                x => x.Id == _currentUser.Id,
                cancellationToken);

        if (user is null)
            throw EntityNotFoundException.For<User>(_currentUser.Id);

        var orders = await _context.Orders
            .Where(order => order.UserId == _currentUser.Id)
            .ToListAsync(cancellationToken);

        if (!orders.Any())
            throw new EntityNotFoundException("No orders found by this user.");

        return new Response(GenerateInvoice(user, orders));
    }

    private Stream GenerateInvoice(User user, IEnumerable<Order> orders)
    {
        var renderer = new ChromePdfRenderer();
        var html = new StringBuilder(File.ReadAllText("Assets/Invoice.html"))
            .Replace("{Order.Id}", _id.ToString())
            .Replace("{Order.Total}", orders.Select(order => order.TotalPrice).Sum().ToString())
            .Replace("{User.FirstName}", user.FirstName)
            .Replace("{User.MiddleName}", user.MiddleName)
            .Replace("{User.LastName}", user.LastName)
            .Replace("{Orders}", string.Join('\n', orders.Select(order => @$"
                            <tr>
                            <td>{order.Id}</td>
                            <td>{order.Name}</td>
                            <td>{order.Amount}</td>
                            <td>{order.Price * order.Period}</td>
                            <td>{order.TotalPrice}</td>
                            </tr>
                            ")));
        _id++;
        return new MemoryStream(renderer.RenderHtmlAsPdf(html.ToString()).BinaryData);
    }
}
