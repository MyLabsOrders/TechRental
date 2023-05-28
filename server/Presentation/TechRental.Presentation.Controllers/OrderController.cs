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
    /// <param name="orderTime">Order's time</param>
    /// <returns>Information about specified order</returns>
    [HttpGet("at")]
    [Authorize]
    public async Task<ActionResult<IList<OrderDto>>> GetOrderAsync([FromQuery] DateTime orderTime)
    {
        var query = new GetOrder.Query(orderTime);
        var response = await _mediator.Send(query);

        return Ok(response.Orders);
    }

    /// <summary>
    /// Get sales chart for a date range
    /// </summary>
    /// <param name="From">Start of range</param>
    /// <param name="To">End of range</param>
    /// <returns>Sales chart as PDF document</returns>
    [HttpGet("stats")]
    [Authorize(Roles = TechRentalIdentityRoleNames.AdminRoleName)]
    [Produces("application/pdf", new string[] { })]
    public async Task<ActionResult> GetStats([FromQuery] DateTime From, [FromQuery] DateTime To)
    {
        var query = new GetStats.Query(From, To);
        var response = await _mediator.Send(query);

        return new FileStreamResult(response.Stream, "application/pdf");
    }

    /// <summary>
    /// Get invoice of the order for current user
    /// </summary>
    /// <param name="orderTime">The time of order</param>
    /// <returns>Invoice as PDF document</returns>
    [HttpGet("invoice")]
    [Authorize]
    [Produces("application/pdf", new string[] { })]
    public async Task<ActionResult> GetInvoice([FromQuery] DateTime orderTime)
    {
        var query = new GetInvoice.Query(orderTime);
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
