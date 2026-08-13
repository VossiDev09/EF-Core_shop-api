namespace EfCore_Uebung.Specifications;

public static class SpecificationsExtensions
{
    public static IQueryable<T> Where<T>(this IQueryable<T> query, Specification<T> spec)
        => query.Where(spec.ToExpression());
}