using EfCore_Uebung.Data;
using EfCore_Uebung.Dtos;
using EfCore_Uebung.Models;
using Microsoft.EntityFrameworkCore;

public static class OrderController
{
    public static void MapOrderController(this WebApplication app)
    {

        app.MapPost("/customers/{id}/orders", async (int id, CreateOrderRequest request, ShopDbContext db) =>
        {
            var customer = await db.Customers.SingleAsync(Customer => Customer.Id == id);

            if (customer == null)
            {
                return Results.NotFound();
            }

            var order = new Order
            {
                CustomerId = id,
                OrderDate = request.OrderDate,
                TotalAmount = request.TotalAmount
            };

            db.Orders.Add(order);
            await db.SaveChangesAsync();

            var response = new OrderResponse(
                order.Id,
                order.OrderDate,
                order.TotalAmount,
                order.CustomerId
            );


            return Results.Ok(response);
        });


        app.MapPut("/orders/{id}", async (int id, decimal newAmount, ShopDbContext db) =>
        {
            var order = await db.Orders.SingleAsync(Order => Order.Id == id);

            if (order == null)
            {
                return Results.NotFound();
            }

            order.TotalAmount = newAmount;

            await db.SaveChangesAsync();

            var response = new OrderResponse(
                order.Id,
                order.OrderDate,
                order.TotalAmount,
                order.CustomerId
            );

            return Results.Ok(response);
        });

        app.MapDelete("/orders/{id}", async (int id, ShopDbContext db) =>
        {
            var order = await db.Orders.SingleAsync(Order => Order.Id == id);

            db.Orders.Remove(order);

            await db.SaveChangesAsync();

            return Results.Ok();
        });

        app.MapGet("/orders/expensive", async (decimal min, ShopDbContext db) =>
        {
            var sortedOrders = await db.Orders.Where(Orders => Orders.TotalAmount > min).OrderByDescending(Order => Order.OrderDate).ToListAsync();

            var response = sortedOrders.Select(Order => new OrderResponse(
                Order.Id,
                Order.OrderDate,
                Order.TotalAmount,
                Order.CustomerId
            ));
            return Results.Ok(response);
        });
    }
}



