namespace EfCore_Uebung.Dtos;

public record OrderResponse(int Id, DateTime OrderDate, decimal TotalAmount, int CustomerId);
public record CreateOrderRequest(DateTime OrderDate, decimal TotalAmount);