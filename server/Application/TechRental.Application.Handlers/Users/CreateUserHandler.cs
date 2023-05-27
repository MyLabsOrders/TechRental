using MediatR;
using TechRental.Application.Abstractions.Identity;
using TechRental.Application.Common.Exceptions;
using TechRental.DataAccess.Abstractions;
using TechRental.Domain.Core.Abstractions;
using TechRental.Domain.Core.Users;
using TechRental.Domain.Core.ValueObject;
using TechRental.Infrastructure.Mapping.Users;
using static TechRental.Application.Contracts.Users.Commands.CreateUser;

namespace TechRental.Application.Handlers.Users;

internal class CreateUserHandler : IRequestHandler<Command, Response>
{
    private readonly IDatabaseContext _context;

    public CreateUserHandler(IDatabaseContext context)
    {
        _context = context;
    }

    public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
    {
        if (!Enum.TryParse(request.Gender, true, out Gender gender))
        {
            throw new InvalidGenderException();
        }
        var user = new User(
            request.IdentityId,
            request.Firstname,
            request.Middlename,
            request.Lastname,
            new Image(request.UserImage),
            request.BirthDate,
            new PhoneNumber(request.PhoneNumber),
            gender);

        _context.Users.Add(user);
        await _context.SaveChangesAsync(cancellationToken);

        return new Response(user.ToDto()!);
    }
}
