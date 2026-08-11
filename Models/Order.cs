namespace EfCore_Uebung.Models;

public class Order
{
    public int Id { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal TotalAmount { get; set; }

    // Fremdschlüssel zum Kunden (n:1)
    public int CustomerId { get; set; }
    public Customer Customer { get; set; } = null!;
}
