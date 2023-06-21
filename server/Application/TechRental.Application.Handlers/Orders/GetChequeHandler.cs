using System.Globalization;
using System.Text;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TechRental.Application.Abstractions.Identity;
using TechRental.DataAccess.Abstractions;
using TechRental.Domain.Common.Exceptions;
using TechRental.Domain.Core.Orders;
using TechRental.Domain.Core.Users;
using static TechRental.Application.Contracts.Orders.Queries.GetCheque;

namespace TechRental.Application.Handlers.Orders;

internal class GetChequeHandler : IRequestHandler<Query, Response>
{
    private readonly IDatabaseContext _context;
    private readonly ICurrentUser _currentUser;

    public GetChequeHandler(IDatabaseContext context, ICurrentUser currentUser)
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
            .Where(order => order.UserId == _currentUser.Id && order.OrderDate == request.OrderDate)
            .ToListAsync(cancellationToken);

        if (!orders.Any())
            throw new EntityNotFoundException("Order with this date is not found by this user.");

        return new Response(GenerateCheque(orders));
    }

    private Stream GenerateCheque(IEnumerable<Order> orders)
    {
        var renderer = new ChromePdfRenderer();
        var html = new StringBuilder(File.ReadAllText("Assets/Cheque.html"))
            .Replace(
                "{Orders}",
                string.Join('\n', orders.Select((order, i) => @$"
                        <p>{order.Name}</p>
                        <p>{i + 1} {order.Amount}x {order.Price * order.Period} ={order.TotalPrice.ToString("C", new CultureInfo("ru-RU"))}</p>"))
            )
            .Replace("{Total}", orders.Sum(order => order.TotalPrice).ToString("C", new CultureInfo("ru-RU")))
            .ToString();
        return new MemoryStream(renderer.RenderHtmlAsPdf(html).BinaryData);
    }
}
