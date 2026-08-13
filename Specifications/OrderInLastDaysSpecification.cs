using System.Linq.Expressions;
using EfCore_Uebung.Models;
using Microsoft.Identity.Client;

namespace EfCore_Uebung.Specifications;

public class OrderInLastDaysSpecification(double? days) : Specification<Order>
{
    public DateTime daysAgo;
    private void days(double? days)
    {
        if (days.HasValue)
        {
            daysAgo = DateTime.UtcNow.AddDays((double)-days);
        }
    }
    public override Expression<Func<Order, bool>> ToExpression()
     => order => order.OrderDate >= daysAgo;
}