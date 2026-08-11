namespace EfCore_Uebung.Models;

public class Customer
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    // Ein Kunde kann mehrere Bestellungen haben (1:n)
    public List<Order> Orders { get; set; } = new();
}
