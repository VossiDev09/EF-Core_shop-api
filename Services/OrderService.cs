using EfCore_Uebung.Data;
using EfCore_Uebung.Models;
using EfCore_Uebung.Specifications;
using Microsoft.EntityFrameworkCore;

public class OrderService(ShopDbContext db) : IOrderService
{
    public async Task<Order> CreateOrder(int id, DateTime orderDate, decimal totalAmount)
    {
        var customer = await db.Customers.SingleOrDefaultAsync(customer => customer.Id == id);

        if (customer == null)
        {
            throw new KeyNotFoundException();
        }

        var order = new Order
        {
            CustomerId = id,
            OrderDate = orderDate,
            TotalAmount = totalAmount
        };

        db.Orders.Add(order);
        await db.SaveChangesAsync();

        return order;
    }

    public async Task DeleteOrder(int id)
    {
        var order = await db.Orders.SingleOrDefaultAsync(order => order.Id == id);

        if (order == null)
        {
            throw new KeyNotFoundException();
        }

        db.Orders.Remove(order);
        await db.SaveChangesAsync();
    }

    public async Task<Order?> GetOrderById(int id)
    {
        var order = await db.Orders.SingleOrDefaultAsync(order => order.Id == id);

        return order;
    }

    public async Task<List<Order>> FilterByAmount(decimal minAmount)
    {
        var sortedOrders = await db.Orders
        .Where(orders => orders.TotalAmount > minAmount)
        .OrderByDescending(order => order.OrderDate)
        .ToListAsync();

        if (sortedOrders == null)
        {
            throw new KeyNotFoundException();
        }

        return sortedOrders;
    }

    public async Task<List<Order>> SearchOrder(int? customerId, decimal? minAmount, int? days)
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
            query = query.Where(new OrderInLastDaysSpecification(days.Value));
        }

        if(query == null)
        {
            throw new KeyNotFoundException();
        }
        
        return await query.ToListAsync();
    }

    public async Task<Order> UpdateOrderAmount(int id, decimal newAmount)
    {
        var order = await db.Orders.SingleOrDefaultAsync(order => order.Id == id);

        if (order == null)
        {
            throw new KeyNotFoundException();
        }

        order.TotalAmount = newAmount;

        await db.SaveChangesAsync();

        return order;
    }
}