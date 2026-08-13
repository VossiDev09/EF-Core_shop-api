using EfCore_Uebung.Data;
using EfCore_Uebung.Dtos;
using EfCore_Uebung.Models;
using EfCore_Uebung.Specifications;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("[Controller]")]
public class OrderController(ShopDbContext db) : ControllerBase
{
    [HttpPost("/customers/{id}/orders")]
    public async Task<ActionResult<OrderResponse>> Create(int id, CreateOrderRequest request)
    {
        var customer = await db.Customers.SingleOrDefaultAsync(customer => customer.Id == id);

        if (customer == null)
        {
            return NotFound();
        }

        var order = new Order
        {
            CustomerId = id,
            OrderDate = request.OrderDate,
            TotalAmount = request.TotalAmount
        };

        db.Orders.Add(order);
        await db.SaveChangesAsync();

        var response = order.ToResponse();

        return CreatedAtAction(nameof(GetById), new {id = order.Id}, response);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<OrderResponse>> UpdateAmount(int id, [FromQuery] decimal newAmount)
    {
        var order = await db.Orders.SingleOrDefaultAsync(order => order.Id == id);

        if (order == null)
        {
            return NotFound();
        }

        order.TotalAmount = newAmount;

        await db.SaveChangesAsync();

        var response = order.ToResponse();

        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var order = await db.Orders.SingleOrDefaultAsync(order => order.Id == id);

        if (order == null)
        {
            return NotFound();
        }

        db.Orders.Remove(order);

        await db.SaveChangesAsync();

        return NoContent();
    }

    [HttpGet("expensive")]
    public async Task<ActionResult<List<OrderResponse>>> FilterByAmount([FromQuery] decimal min)
    {
        var sortedOrders = await db.Orders
        .Where(orders => orders.TotalAmount > min)
        .OrderByDescending(order => order.OrderDate)
        .ToListAsync();

        var response = sortedOrders.Select(order => order.ToResponse()).ToList();
        return Ok(response);
    }
    
    [HttpGet]
    public async Task<ActionResult<OrderResponse>> GetById([FromQuery] int id)
    {
        var order = await db.Orders.SingleOrDefaultAsync(order => order.Id == id);

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
        var query = db.Orders.AsQueryable();

        if (customerId.HasValue)
        {
            query = query.Where(new OrderByCustomerSpecification(customerId));
        }

        if (minAmount.HasValue)
        {
            query = query.Where(new OrderByMinAmountSpecification(minAmount));
        }

        if (days.HasValue)
        {
            query = query.Where(new OrderInLastDaysSpecification(days));
        }

        var response = await query.ToListAsync();

        return Ok(response);
    }
}




