namespace EfCore_Uebung.Dtos;

public record CreateCustomerRequest(string Name, string Email);
public record CustomerResponse(string Name, string Email, List<OrderResponse> Orders);
public record OrderResponse(int Id, DateTime OrderDate, decimal TotalAmount, int CustomerId);
public record CreateOrderRequest(DateTime OrderDate, decimal TotalAmount);