using EfCore_Uebung.Data;
using EfCore_Uebung.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

// DbContext per Dependency Injection registrieren (API-Standard)
builder.Services.AddDbContext<ShopDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
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
app.MapPost("/customers", async (Customer customer, ShopDbContext db) =>
{
    // TODO: customer zu db.Customers hinzufügen, speichern (SaveChangesAsync)
    //       und Results.Created(...) mit dem angelegten Kunden zurückgeben.
    return Results.Problem("Aufgabe 1 noch nicht implementiert.");
});

// ---------------------------------------------------------------------
// AUFGABE 2: Alle Kunden inkl. Bestellungen lesen (READ)  ->  GET /customers
// ---------------------------------------------------------------------
app.MapGet("/customers", async (ShopDbContext db) =>
{
    // TODO: Alle Customers MIT ihren Orders laden (Include!) und zurückgeben.
    return Results.Problem("Aufgabe 2 noch nicht implementiert.");
});

// ---------------------------------------------------------------------
// AUFGABE 3: Einen Kunden per Id lesen (READ)  ->  GET /customers/{id}
// ---------------------------------------------------------------------
app.MapGet("/customers/{id}", async (int id, ShopDbContext db) =>
{
    // TODO: Kunden per Id (inkl. Orders) suchen. Gefunden -> Ok, sonst NotFound.
    return Results.Problem("Aufgabe 3 noch nicht implementiert.");
});

// ---------------------------------------------------------------------
// AUFGABE 4: Bestellung zu einem Kunden anlegen (CREATE)
//            ->  POST /customers/{id}/orders
// ---------------------------------------------------------------------
app.MapPost("/customers/{id}/orders", async (int id, Order order, ShopDbContext db) =>
{
    // TODO: Prüfen ob Kunde existiert. order.CustomerId setzen, speichern.
    return Results.Problem("Aufgabe 4 noch nicht implementiert.");
});

// ---------------------------------------------------------------------
// AUFGABE 5: Bestellbetrag ändern (UPDATE)  ->  PUT /orders/{id}
// ---------------------------------------------------------------------
app.MapPut("/orders/{id}", async (int id, decimal newAmount, ShopDbContext db) =>
{
    // TODO: Order laden, TotalAmount setzen, speichern. Sonst NotFound.
    return Results.Problem("Aufgabe 5 noch nicht implementiert.");
});

// ---------------------------------------------------------------------
// AUFGABE 6: Bestellung löschen (DELETE)  ->  DELETE /orders/{id}
// ---------------------------------------------------------------------
app.MapDelete("/orders/{id}", async (int id, ShopDbContext db) =>
{
    // TODO: Order laden, mit db.Orders.Remove(...) entfernen, speichern.
    return Results.Problem("Aufgabe 6 noch nicht implementiert.");
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
