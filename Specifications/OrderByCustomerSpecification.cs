using System.Linq.Expressions;
using EfCore_Uebung.Models;

namespace EfCore_Uebung.Specifications;

public class OrderByCustomerSpecification(int? customerId) : Specification<Order>
{
    public override Expression<Func<Order, bool>> ToExpression()
        => order => order.CustomerId == customerId;
}