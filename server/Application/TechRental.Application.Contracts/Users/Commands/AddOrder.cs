using MediatR;

namespace TechRental.Application.Contracts.Users.Commands;

internal static class AddOrder
{
    public record Command(Guid UserId, IList<(Guid OrderId, int Amount, int Days)> Orders) : IRequest;
}
