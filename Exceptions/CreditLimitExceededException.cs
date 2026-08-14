namespace EfCore_Uebung.Exceptions;

public class CreditLimitExceededException : BusinessRuleException
{
    public CreditLimitExceededException(decimal available)
        : base($"Kreditlimit überschritten. Verfügbar: {available:C}.") { }
}