namespace EfCore_Uebung.Exceptions;

public class BusinessRuleException : Exception
{
    public string ErrorCode { get; }
    public int StatusCode { get; }      

    public BusinessRuleException(string message)
        : base(message)
    {
        ErrorCode = "CONFLICT";
        StatusCode = StatusCodes.Status409Conflict;
    }
}