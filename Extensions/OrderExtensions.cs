using EfCore_Uebung.Dtos;
using EfCore_Uebung.Models;

public static class OrderExtensions
{
    public static OrderResponse ToResponse(this Order order)
    {
        return new OrderResponse(
            order.Id,
            order.OrderDate,
            order.TotalAmount,
            order.CustomerId
        );
    }
}