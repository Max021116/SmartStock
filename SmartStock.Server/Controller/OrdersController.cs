using Microsoft.AspNetCore.Mvc;
using SmartStock.Server.Exceptions;
using SmartStock.Server.Services;
using SmartStock.Shared;
using SmartStock.Server.Data;
using Microsoft.EntityFrameworkCore;

namespace SmartStock.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;
    private readonly AppDbContext _context;  // only to read invoice after place — or extend OrderService return

    public OrdersController(IOrderService orderService, AppDbContext context)
    {
        _orderService = orderService;
        _context = context;
    }

    [HttpPost]
    public async Task<ActionResult<OrderPlacedDto>> PlaceOrder(
        [FromBody] PlaceOrderRequest request,
        CancellationToken cancellationToken)
    {
        if (request.Items.Count == 0)
            return BadRequest(new ApiErrorResponse { Message = "Order must have at least one item." });

        try
        {
            var lines = request.Items
                .Select(i => new PlaceOrderLine(i.ProductId, i.Quantity, i.UnitPrice))
                .ToList();

            var order = await _orderService.PlaceSalesOrderAsync(
                request.CustomerId, lines, cancellationToken);

            var invoice = await _context.Invoices
                .AsNoTracking()
                .FirstAsync(i => i.SalesOrderId == order.Id, cancellationToken);

            return CreatedAtAction(nameof(PlaceOrder), new { id = order.Id }, new OrderPlacedDto
            {
                OrderId = order.Id,
                TotalAmount = order.TotalAmount,
                InvoiceNumber = invoice.InvoiceNumber
            });
        }
        catch (InsufficientStockException ex)
        {
            return BadRequest(new ApiErrorResponse { Message = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new ApiErrorResponse { Message = ex.Message });
        }
    }
}