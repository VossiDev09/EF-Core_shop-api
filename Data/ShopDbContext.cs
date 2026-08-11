using EfCore_Uebung.Models;
using Microsoft.EntityFrameworkCore;

namespace EfCore_Uebung.Data;

public class ShopDbContext(DbContextOptions<ShopDbContext> options) : DbContext(options)
{
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Order> Orders { get; set; }
}
