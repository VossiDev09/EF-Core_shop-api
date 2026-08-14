using EfCore_Uebung.Data;
using EfCore_Uebung.Models;
using EfCore_Uebung.Specifications;
using Microsoft.EntityFrameworkCore;
using EfCore_Uebung.Exceptions;

public class OrderService(ShopDbContext db) : IOrderService
{
    private static readonly Dictionary<OrderStatus, OrderStatus[]> Allowed = new()
    {
        [OrderStatus.Pending]   = [OrderStatus.Paid, OrderStatus.Cancelled],
        [OrderStatus.Paid]      = [OrderStatus.Shipped, OrderStatus.Cancelled],
        [OrderStatus.Shipped]   = [],
        [OrderStatus.Cancelled] = [],
    };

    public async Task<Order> CreateOrder(int id, DateTime orderDate, decimal totalAmount)
    {
        var customer = await db.Customers.SingleOrDefaultAsync(customer => customer.Id == id);

        if (customer == null)
        {
            throw new NotFoundException("Customer", id);
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
            throw new NotFoundException("Order", id);
        }

        db.Orders.Remove(order);
        await db.SaveChangesAsync();
    }

    public async Task<Order?> GetOrderById(int id)
    {
        return await db.Orders.SingleOrDefaultAsync(order => order.Id == id);
    }

    public async Task<List<Order>> FilterByAmount(decimal minAmount)
    {
        var sortedOrders = await db.Orders
        .Where(orders => orders.TotalAmount > minAmount)
        .OrderByDescending(order => order.OrderDate)
        .ToListAsync();

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

        return await query.ToListAsync();
    }

    public async Task<Order> UpdateOrderAmount(int id, decimal newAmount)
    {
        var order = await db.Orders.SingleOrDefaultAsync(order => order.Id == id);

        if (order == null)
        {
            throw new NotFoundException("Order", id);
        }

        order.TotalAmount = newAmount;

        await db.SaveChangesAsync();

        return order;
    }

    public async Task<Order> UpdateOrderStatus(int id, OrderStatus newStatus)
    {
        var order = await db.Orders.SingleOrDefaultAsync(order => order.Id == id);
        
        if (order == null)
        {
            throw new NotFoundException("order", id);
        } 
        else if(!Allowed[order.OrderStatus].Contains(newStatus))
        {
            throw new InvalidOrderStatusTransitionException(order.OrderStatus, newStatus);
        }     
        else
        {
            order.OrderStatus = newStatus;
            await db.SaveChangesAsync();
            return order;
        }
    }
}