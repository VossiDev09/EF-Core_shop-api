using EfCore_Uebung.Dtos;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[Controller]")]
public class OrderController(IOrderService orderService) : ControllerBase
{
    [HttpPost("/customers/{id}/orders")]
    public async Task<ActionResult<OrderResponse>> Create(int id, CreateOrderRequest request)
    {
        var order = await orderService.CreateOrder(id, request.OrderDate, request.TotalAmount);
        var response = order.ToResponse();

        return CreatedAtAction(nameof(GetById), new { id = order.Id }, response);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<OrderResponse>> UpdateAmount(int id, [FromQuery] decimal newAmount)
    {
        var order = await orderService.UpdateOrderAmount(id, newAmount);
        var response = order.ToResponse();

        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        await orderService.DeleteOrder(id);

        return NoContent();
    }

    [HttpGet("expensive")]
    public async Task<ActionResult<List<OrderResponse>>> FilterByAmount([FromQuery] decimal min)
    {
        var sortedOrders = await orderService.FilterByAmount(min);
        var response = sortedOrders.Select(order => order.ToResponse()).ToList();
        
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<OrderResponse>> GetById([FromQuery] int id)
    {
        var order = await orderService.GetOrderById(id);

        if (order == null)
        {
            return NotFound();
        }

        var response = order.ToResponse();

        return Ok(response);
    }

    [HttpGet("search")]
    public async Task<ActionResult<List<OrderResponse>>> Search(
        [FromQuery] int? customerId,
        [FromQuery] decimal? minAmount,
        [FromQuery] int? days)
    {
        var query = await orderService.SearchOrder(customerId, minAmount, days);

        return Ok(query.ToResponseList());
    }
}




