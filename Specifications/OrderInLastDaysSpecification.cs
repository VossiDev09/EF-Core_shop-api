using System.Linq.Expressions;
using EfCore_Uebung.Models;

namespace EfCore_Uebung.Specifications;

public class OrderInLastDaysSpecification : Specification<Order>
{
    private DateTime daysAgo;
    public OrderInLastDaysSpecification(int days)
        => daysAgo = DateTime.UtcNow.AddDays(-days);

    public override Expression<Func<Order, bool>> ToExpression()
     => order => order.OrderDate >= daysAgo;
}