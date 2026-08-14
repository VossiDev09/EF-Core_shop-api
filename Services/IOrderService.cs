using EfCore_Uebung.Dtos;
using EfCore_Uebung.Models;

public interface IOrderService
{
    Task<Order> CreateOrder(int id, DateTime orderDate, decimal totalAmount);
    Task DeleteOrder(int id);
    Task<Order?> GetOrderById(int id);
    Task<List<Order>> FilterByAmount(decimal minAmount);
    Task<List<Order>> SearchOrder(int? customerId, decimal? minAmount, int? days);
    Task<Order> UpdateOrderAmount(int id, decimal newAmount);
}