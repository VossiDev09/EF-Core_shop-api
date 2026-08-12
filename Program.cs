using System.Runtime.CompilerServices;
using EfCore_Uebung.Data;
using EfCore_Uebung.Dtos;
using EfCore_Uebung.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

// DbContext per Dependency Injection registrieren (API-Standard)
builder.Services.AddDbContext<ShopDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

// =====================================================================
//  EF Core Übung – Online-Shop API (Customer + Order)
//  Implementiere die Endpunkte der Reihe nach. Der DbContext kommt
//  jeweils per Parameter (DI) in den Handler. Teste über die
//  OpenAPI-/.http-Datei oder mit curl.
// =====================================================================

// ---------------------------------------------------------------------
// AUFGABE 1: Kunden anlegen (CREATE)  ->  POST /customers
// ---------------------------------------------------------------------
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

// ---------------------------------------------------------------------
// AUFGABE 2: Alle Kunden inkl. Bestellungen lesen (READ)  ->  GET /customers
// ---------------------------------------------------------------------
app.MapGet("/customers", async (ShopDbContext db) =>
{
    var customers = await db.Customers.Include(Customers => Customers.Orders).ToListAsync();

    var response = customers.Select(Customer => new CustomerResponse(
        Customer.Id,
        Customer.Name,
        Customer.Email,
        Customer.Orders.Select(Order => new OrderResponse(
            Order.Id, Order.OrderDate, Order.TotalAmount, Order.CustomerId)).ToList())).ToList();

    // TODO: Alle Customers MIT ihren Orders laden (Include!) und zurückgeben.
    return Results.Ok(response);
});

// ---------------------------------------------------------------------
// AUFGABE 3: Einen Kunden per Id lesen (READ)  ->  GET /customers/{id}
// ---------------------------------------------------------------------
app.MapGet("/customers/{id}", async (int id, ShopDbContext db) =>
{
    var customer = await db.Customers.Include(Customer => Customer.Orders).SingleOrDefaultAsync(Customer => Customer.Id == id);

    if(customer == null)
    {
        return Results.NotFound();
    }

    var response = new CustomerResponse(
        customer.Id,
        customer.Name,
        customer.Email,
        customer.Orders.Select(Orders => new OrderResponse(
            Orders.Id, Orders.OrderDate, Orders.TotalAmount, Orders.CustomerId)).ToList());
    // TODO: Kunden per Id (inkl. Orders) suchen. Gefunden -> Ok, sonst NotFound.
    return Results.Ok(response);
});

// ---------------------------------------------------------------------
// AUFGABE 4: Bestellung zu einem Kunden anlegen (CREATE)
//            ->  POST /customers/{id}/orders
// ---------------------------------------------------------------------
app.MapPost("/customers/{id}/orders", async (int id, CreateOrderRequest request, ShopDbContext db) =>
{
    var customer = await db.Customers.SingleAsync(Customer => Customer.Id == id);

    if(customer == null)
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

    // TODO: Prüfen ob Kunde existiert. order.CustomerId setzen, speichern.
    return Results.Ok(response);
});

// ---------------------------------------------------------------------
// AUFGABE 5: Bestellbetrag ändern (UPDATE)  ->  PUT /orders/{id}
// ---------------------------------------------------------------------
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

    // TODO: Order laden, TotalAmount setzen, speichern. Sonst NotFound.
    return Results.Ok(response);
});

// ---------------------------------------------------------------------
// AUFGABE 6: Bestellung löschen (DELETE)  ->  DELETE /orders/{id}
// ---------------------------------------------------------------------
app.MapDelete("/orders/{id}", async (int id, ShopDbContext db) =>
{
    var order = await db.Orders.SingleAsync(Order => Order.Id == id);

    db.Orders.Remove(order);

    await db.SaveChangesAsync();

    // TODO: Order laden, mit db.Orders.Remove(...) entfernen, speichern.
    return Results.Ok();
});

// ---------------------------------------------------------------------
// AUFGABE 7 (Bonus): Teure Bestellungen filtern (LINQ)
//            ->  GET /orders/expensive?min=50
// ---------------------------------------------------------------------
app.MapGet("/orders/expensive", async (decimal min, ShopDbContext db) =>
{
    // TODO: Alle Orders mit TotalAmount > min, sortiert nach OrderDate absteigend.
    return Results.Problem("Aufgabe 7 noch nicht implementiert.");
});

app.Run();
