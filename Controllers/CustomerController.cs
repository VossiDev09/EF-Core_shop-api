using EfCore_Uebung.Data;
using EfCore_Uebung.Dtos;
using EfCore_Uebung.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("[Controller]")]

public class CustomerController(ShopDbContext db) : ControllerBase
{


    [HttpPost]
    public async Task<ActionResult<CustomerResponse>> Create(CreateCustomerRequest request)
    {
        var customer = new Customer
        {
            Name = request.Name,
            Email = request.Email
        };
        db.Customers.Add(customer);
        await db.SaveChangesAsync();
        var response = customer.ToResponse();

        return CreatedAtAction(nameof(GetById), new { id = customer.Id }, customer.ToResponse());
    }

    [HttpGet]
    public async Task<ActionResult<List<CustomerResponse>>> GetAll()
    {
        var customers = await db.Customers.Include(customers => customers.Orders).ToListAsync();

        var response = customers.Select(customer => customer.ToResponse()).ToList();

        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<CustomerResponse>> GetById(int id)
    {
        var customer = await db.Customers.Include(customer => customer.Orders).SingleOrDefaultAsync(customer => customer.Id == id);

        if (customer == null)
        {
            return NotFound();
        }

        var response = customer.ToResponse();
        return Ok(response);
    }
}