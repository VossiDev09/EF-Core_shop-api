namespace EfCore_Uebung.Exceptions;

public class BusinessRuleException : Exception
{
    public string ErrorCode { get; }
    public int StatusCode { get; }      

    public BusinessRuleException()
        : base()
    {
        ErrorCode = "CONFLICT";
        StatusCode = StatusCodes.Status409Conflict;
    }
}