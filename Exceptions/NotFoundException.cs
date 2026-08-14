namespace EfCore_Uebung.Exceptions;

public class NotFoundException : Exception
{
    public string ErrorCode { get; }
    public int StatusCode { get; }      

    public NotFoundException(string entityName, int key)
        : base($"{entityName} with ID '{key}' wasn't found.")
    {
        ErrorCode = "NOT_FOUND";
        StatusCode = StatusCodes.Status404NotFound;
    }
}