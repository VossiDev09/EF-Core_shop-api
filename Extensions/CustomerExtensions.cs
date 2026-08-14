using EfCore_Uebung.Dtos;
using EfCore_Uebung.Models;


public static class CustomerExtensions
{
    public static CustomerResponse ToResponse(this Customer customer)
    {
        return new CustomerResponse(
            customer.Id,
            customer.Name,
            customer.Email,
            customer.Orders.ToResponseList());
    }
}