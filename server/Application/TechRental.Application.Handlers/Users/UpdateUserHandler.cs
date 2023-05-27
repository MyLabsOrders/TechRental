using MediatR;
using Microsoft.EntityFrameworkCore;
using TechRental.DataAccess.Abstractions;
using TechRental.Domain.Common.Exceptions;
using TechRental.Domain.Core.Abstractions;
using TechRental.Domain.Core.Users;
using TechRental.Domain.Core.ValueObject;
using TechRental.Infrastructure.Mapping.Users;
using static TechRental.Application.Contracts.Users.Commands.UpdateUser;

namespace TechRental.Application.Handlers.Users;

internal class UpdateUserHandler : IRequestHandler<Command, Response>
{
    private readonly IDatabaseContext _context;

    public UpdateUserHandler(IDatabaseContext context)
    {
        _context = context;
    }

    public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FirstAsync(x => x.Id.Equals(request.IdentityId), cancellationToken);

        if (user is null)
            throw EntityNotFoundException.For<User>(request.IdentityId);

        if (request.Firstname is not null)
            user.FirstName = request.Firstname;
        if (request.Middlename is not null)
            user.MiddleName = request.Middlename;
        if (request.Lastname is not null)
            user.LastName = request.Lastname;
        if (request.PhoneNumber is not null)
            user.PhoneNumber = new PhoneNumber(request.PhoneNumber);
        if (request.UserImage is not null)
            user.Image = new Image(request.UserImage);
        if (request.BirthDate is not null)
            user.BirthDate = (DateOnly)request.BirthDate;
        if (request.Gender is not null)
            user.Gender = Enum.Parse<Gender>(request.Gender, true);
        if (request.IsActive is not null)
            user.IsActive = (bool)request.IsActive;

        await _context.SaveChangesAsync(cancellationToken);

        return new Response(user.ToDto()!);
    }
}
