﻿using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechRental.Application.Abstractions.Identity;
using TechRental.Application.Contracts.Users.Commands;
using TechRental.Application.Contracts.Users.Queries;
using TechRental.Application.Dto.Users;
using TechRental.Presentation.Contracts.Users;

namespace TechRental.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IMediator _mediator;

    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Creates user account with all his personnel data
    /// </summary>
    /// <param name="request"></param>
    /// <returns>Information about created account</returns>
    [HttpPost("profile")]
    [Authorize]
    public async Task<ActionResult<UserDto>> CreateUserAsync([FromBody] CreateUserRequest request)
    {
        var command = new CreateUser.Command(
            request.FirstName,
            request.MiddleName,
            request.LastName,
            Convert.FromBase64String(request.UserImage ?? string.Empty),
            request.BirthDate,
            request.PhoneNumber);

        var response = await _mediator.Send(command);

        return Ok(response);
    }

    /// <summary>
    /// Adds order to user purchase bucket
    /// </summary>
    /// <param name="userId">Target user id</param>
    /// <param name="request">Target order id</param>
    /// <returns></returns>
    [HttpPut("{userId:guid}/orders")]
    [Authorize(Roles = TechRentalIdentityRoleNames.DefaultUserRoleName)]
    public async Task<IActionResult> AddOrderAsync(Guid userId, [FromBody] AddOrderRequest request)
    {
        var command = new AddOrder.Command(userId, request.OrderId);
        await _mediator.Send(command);

        return Ok();
    }

    /// <summary>
    /// Removes order from user purchase bucket
    /// </summary>
    /// <param name="userId">Target user id</param>
    /// <param name="request">Target order id</param>
    /// <returns></returns>
    [HttpDelete("{userId:guid}/orders")]
    [Authorize(Roles = TechRentalIdentityRoleNames.DefaultUserRoleName)]
    public async Task<IActionResult> RemoveOrderAsync(Guid userId, [FromBody] RemoveOrderRequest request)
    {
        var command = new RemoveOrder.Command(request.OrderId, userId);
        await _mediator.Send(command);

        return Ok();
    }

    /// <summary>
    /// Gets specified user
    /// </summary>
    /// <param name="id">Target user id</param>
    /// <returns>Information about specified user</returns>
    [HttpGet("{id:guid}")]
    [Authorize(Roles = TechRentalIdentityRoleNames.AdminRoleName)]
    public async Task<ActionResult<UserDto>> GetUserAsync(Guid id)
    {
        var query = new GetUser.Query(id);
        var response = await _mediator.Send(query);

        return Ok(response.User);
    }

    /// <summary>
    /// Gets all users
    /// </summary>
    /// <returns>All users</returns>
    [HttpGet]
    [Authorize(Roles = TechRentalIdentityRoleNames.AdminRoleName)]
    public async Task<ActionResult<UserDto>> GetUsersAsync()
    {
        var query = new GetAllUsers.Query();
        var response = await _mediator.Send(query);

        return Ok(response.Users);
    }

    /// <summary>
    /// Make deposit to customer account
    /// </summary>
    /// <param name="id">Target customer id</param>
    /// <param name="request">Money amount to be replenished</param>
    /// <returns></returns>
    [HttpPut("{id:guid}/account")]
    [Authorize]
    public async Task<IActionResult> MakeDepositAsync(Guid id, [FromBody] ReplenishBalanceRequest request)
    {
        var command = new ReplenishBalance.Command(id, request.Total);
        await _mediator.Send(command);

        return Ok();
    }

}
