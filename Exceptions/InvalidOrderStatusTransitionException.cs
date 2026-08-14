namespace EfCore_Uebung.Exceptions;

public class InvalidOrderStatusTransitionException : BusinessRuleException
{
    public InvalidOrderStatusTransitionException(OrderStatus from, OrderStatus to)
        : base($"Statuswechsel von '{from}' nach '{to}' ist nicht erlaubt.") { }
}