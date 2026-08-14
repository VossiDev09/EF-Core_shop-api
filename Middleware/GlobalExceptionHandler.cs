using EfCore_Uebung.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var (statusCode, errorCode) = exception switch
        {
            NotFoundException ex => (ex.StatusCode, ex.ErrorCode),
            BusinessRuleException ex => (ex.StatusCode, ex.ErrorCode),
            _                        => (StatusCodes.Status500InternalServerError, "INTERNAL_ERROR")
        };

        if (statusCode >= StatusCodes.Status500InternalServerError)
            logger.LogError(exception, "Unbehandelte Exception");

        var problem = new ProblemDetails
        {
          Status = statusCode,
          Title = errorCode,
          Detail = exception is NotFoundException or BusinessRuleException
          ? exception.Message
          : "An internal error has occurred."  
        };
        problem.Extensions["errorCode"] = errorCode;

        httpContext.Response.StatusCode = statusCode;
        await httpContext.Response.WriteAsJsonAsync(problem, cancellationToken);

        return true;
    }
}