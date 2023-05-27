using MediatR;
using TechRental.Application.Dto.Users;

namespace TechRental.Application.Contracts.Users.Commands;

internal static class CreateUser
{
    public record Command(Guid IdentityId, string Firstname, string Middlename, string Lastname, string UserImage, DateOnly BirthDate,
        string PhoneNumber, string Gender) : IRequest<Response>;

    public record Response(UserDto User);
}
