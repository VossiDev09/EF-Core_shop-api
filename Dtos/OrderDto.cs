namespace EfCore_Uebung.Dtos;

public record OrderResponse(int Id, OrderStatus OrderStatus, DateTime OrderDate, decimal TotalAmount, int CustomerId);
public record CreateOrderRequest(DateTime OrderDate, decimal TotalAmount);
public record UpdateOrderStatusRequest(OrderStatus OrderStatus);