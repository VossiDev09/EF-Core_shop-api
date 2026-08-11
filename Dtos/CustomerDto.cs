namespace EfCore_Uebung.Dtos;

public record CreateCustomerRequest(string Name, string Email);
public record CustomerResponse(int Id, string Name, string Email, List<OrderResponse> Orders);
