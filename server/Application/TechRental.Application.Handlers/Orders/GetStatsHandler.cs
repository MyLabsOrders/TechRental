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
using static TechRental.Application.Contracts.Orders.Queries.GetStats;

namespace TechRental.Application.Handlers.Orders;

internal class GetStatsHandler : IRequestHandler<Query, Response>
{
    private readonly IDatabaseContext _context;
    private readonly ICurrentUser _currentUser;
    private static int _id = 1;

    public GetStatsHandler(IDatabaseContext context, ICurrentUser currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
    {
        var orders = await _context.Orders
            .Where(order => request.From.ToUniversalTime() <= order.OrderDate && order.OrderDate <= request.To.ToUniversalTime())
            .ToListAsync(cancellationToken);

        if (!orders.Any())
            throw new EntityNotFoundException("Orders within this date range were not found");

        return new Response(GenerateStats(orders));
    }

    private Stream GenerateStats(IEnumerable<Order> orders)
    {
        var renderer = new ChromePdfRenderer
        {
            RenderingOptions = new ChromePdfRenderOptions
            {
                EnableJavaScript = true,
                CssMediaType = IronPdf.Rendering.PdfCssMediaType.Print
            }
        };
        var stats = orders.GroupBy(order => order.Name).Select(orderGroup => new
        {
            Name = orderGroup.Key,
            Value = orderGroup.Sum(order => order.Amount)
        });
        var html = new StringBuilder(File.ReadAllText("Assets/Chart.html"))
            .Replace("{Statistics}", string.Join(
                ",",
                stats.Select(s => $"{{name:'{s.Name}',value:{s.Value}}}")
            ))
            .ToString();
        _id++;
        return new MemoryStream(renderer.RenderHtmlAsPdf(html).BinaryData);
    }
}
