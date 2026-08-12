using EfCore_Uebung.Data;
using EfCore_Uebung.Dtos;
using EfCore_Uebung.Models;
using Microsoft.EntityFrameworkCore;

public static class CustomerController
{
    public static void MapCustomerController(this WebApplication app)
    {

        app.MapPost("/customers", async (CreateCustomerRequest request, ShopDbContext db) =>
        {
            var customer = new Customer
            {
                Name = request.Name,
                Email = request.Email
            };

            db.Customers.Add(customer);
            await db.SaveChangesAsync();
            var response = new CustomerResponse(
                customer.Id,
                customer.Name,
                customer.Email,
                customer.Orders.Select(Orders => new OrderResponse(
                    Orders.Id, Orders.OrderDate, Orders.TotalAmount, Orders.CustomerId)).ToList());
            return Results.Created($"/customers/{customer.Id}", response);
        });

        app.MapGet("/customers", async (ShopDbContext db) =>
        {
            var customers = await db.Customers.Include(Customers => Customers.Orders).ToListAsync();

            var response = customers.Select(Customer => new CustomerResponse(
                Customer.Id,
                Customer.Name,
                Customer.Email,
                Customer.Orders.Select(Order => new OrderResponse(
                    Order.Id, Order.OrderDate, Order.TotalAmount, Order.CustomerId)).ToList())).ToList();

            return Results.Ok(response);
        });

        app.MapGet("/customers/{id}", async (int id, ShopDbContext db) =>
        {
            var customer = await db.Customers.Include(Customer => Customer.Orders).SingleOrDefaultAsync(Customer => Customer.Id == id);

            if (customer == null)
            {
                return Results.NotFound();
            }

            var response = new CustomerResponse(
                customer.Id,
                customer.Name,
                customer.Email,
                customer.Orders.Select(Orders => new OrderResponse(
                    Orders.Id, Orders.OrderDate, Orders.TotalAmount, Orders.CustomerId)).ToList());
            return Results.Ok(response);
        });
    }
}