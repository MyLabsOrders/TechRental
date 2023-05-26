using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using TechRental.Application.Abstractions.Identity;
using TechRental.Application.Contracts.Orders.Commands;
using TechRental.Application.Contracts.Orders.Queries;
using TechRental.Application.Dto.Orders;
using TechRental.Presentation.Contracts.Orders;

namespace TechRental.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderController : ControllerBase
{
    private readonly IMediator _mediator;

    public OrderController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Changes order status to the provided one
    /// </summary>
    /// <param name="request">Order id and new status</param>
    /// <returns></returns>
    [HttpPut("status")]
    [Authorize(Roles = TechRentalIdentityRoleNames.AdminRoleName)]
    public async Task<IActionResult> ChangeOrderStatusAsync([FromBody] ChangeOrderStatusRequest request)
    {
        var command = new ChangeOrderStatus.Command(request.OrderId, request.Status);
        await _mediator.Send(command);

        return Ok();
    }

    /// <summary>
    /// Registers new order in the system
    /// </summary>
    /// <param name="request">Order information</param>
    /// <returns>Information about created order</returns>
    [HttpPost]
    [Authorize(Roles = TechRentalIdentityRoleNames.AdminRoleName)]
    public async Task<ActionResult<OrderDto>> CreateOrderAsync([FromBody] CreateOrderRequest request)
    {
        var command = new CreateOrder.Command(
            request.Name,
            request.Company,
            request.OrderImage ?? string.Empty,
            request.Status,
            request.Price);
        var response = await _mediator.Send(command);

        return Ok(response.Order);
    }

    /// <summary>
    /// Gets specified order
    /// </summary>
    /// <param name="orderId">Target order id</param>
    /// <returns>Information about specified order</returns>
    [HttpGet("{orderId:guid}")]
    [Authorize]
    public async Task<ActionResult<OrderDto>> GetOrderAsync(Guid orderId)
    {
        var query = new GetOrder.Query(orderId);
        var response = await _mediator.Send(query);

        return Ok(response.Order);
    }

    [HttpGet("invoice")]
    [Authorize]
    public async Task<ActionResult> GetInvoice()
    {
        var query = new GetInvoice.Query();
        var response = await _mediator.Send(query);

        return new FileStreamResult(response.Stream, "application/pdf");
    }
    /// <summary>
    /// Lists all orders registered in the system
    /// </summary>
    /// <returns>Information about all orders</returns>
    [HttpGet]
    public async Task<ActionResult<GetAllOrdersResponse>> GetAllOrdersAsync(int? page)
    {
        var query = new GetAllOrders.Query(page ?? 1);
        var response = await _mediator.Send(query);

        var getAllOrdersResponse = new GetAllOrdersResponse(
            response.Orders,
            response.Page,
            response.TotalPages);

        return Ok(getAllOrdersResponse);
    }
}
