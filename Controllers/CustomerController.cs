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
            var response = customer.ToResponse();
            return Results.Created($"/customers/{customer.Id}", response);
        });

        app.MapGet("/customers", async (ShopDbContext db) =>
        {
            var customers = await db.Customers.Include(customers => customers.Orders).ToListAsync();

            var response = customers.Select(customer => customer.ToResponse()).ToList();

            return Results.Ok(response);
        });

        app.MapGet("/customers/{id}", async (int id, ShopDbContext db) =>
        {
            var customer = await db.Customers.Include(customer => customer.Orders).SingleOrDefaultAsync(customer => customer.Id == id);

            if (customer == null)
            {
                return Results.NotFound();
            }

            var response = customer.ToResponse();
            return Results.Ok(response);
        });
    }
}