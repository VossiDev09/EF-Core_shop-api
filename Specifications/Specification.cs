using System.Linq.Expressions;

namespace EfCore_Uebung.Specifications;

public abstract class Specification<T>
{
    public abstract Expression<Func<T, bool>> ToExpression();
}