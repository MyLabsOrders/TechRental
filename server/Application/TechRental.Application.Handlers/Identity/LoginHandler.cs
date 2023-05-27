using MediatR;
using TechRental.Application.Abstractions.Identity;
using TechRental.Domain.Common.Exceptions;
using static TechRental.Application.Contracts.Identity.Queries.Login;

namespace TechRental.Application.Handlers.Identity;

internal class LoginHandler : IRequestHandler<Query, Response>
{
    private readonly IAuthorizationService _authorizationService;

    public LoginHandler(IAuthorizationService authorizationService)
    {
        _authorizationService = authorizationService;
    }

    public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
    {
        var user = await _authorizationService.GetUserByNameAsync(request.Username, cancellationToken);

        var passwordCorrect = await _authorizationService.CheckUserPasswordAsync(
            user.Id,
            request.Password,
            cancellationToken);

        if (passwordCorrect is false)
            throw new UnauthorizedException("You are not authorized");

        var token = await _authorizationService.GetUserTokenAsync(request.Username, cancellationToken);

        return new Response(user.Id, token);
    }
}
