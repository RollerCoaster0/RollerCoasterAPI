using System.Net;
using Microsoft.AspNetCore.Mvc;
using RollerCoaster.Services.Abstractions.Common;

namespace RollerCoaster;

public class ExceptionToProblemDetailsHandler(ILogger<ExceptionToProblemDetailsHandler> logger):
    Microsoft.AspNetCore.Diagnostics.IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is NotFoundError notFoundErrorException)
        {
            httpContext.Response.StatusCode = 404;
            await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
            {
                Title = notFoundErrorException.Message,
                Status = (int)HttpStatusCode.NotFound
            }, cancellationToken: cancellationToken); 
        }
        
        else if (exception is AccessDeniedError accessDeniedErrorException)
        {
            httpContext.Response.StatusCode = 403;
            await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
            {
                Title = accessDeniedErrorException.Message,
                Status = (int)HttpStatusCode.Forbidden
            }, cancellationToken: cancellationToken);
        }
            
        
        else if (exception is ProvidedDataIsInvalidError providedDataIsInvalidErrorException)
        {
            httpContext.Response.StatusCode = 400;
            await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
            {
                Title = providedDataIsInvalidErrorException.Message,
                Status = (int)HttpStatusCode.Forbidden
            }, cancellationToken: cancellationToken);
        }
        
        else
        {
            logger.LogError(exception, "Произошла неизвестная ошибка");
            
            httpContext.Response.StatusCode = 500;
            await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
            {
                Title = "Произошла неизвестная ошибка. Попробуйте позже",
                Status = (int)HttpStatusCode.InternalServerError
            }, cancellationToken: cancellationToken);
        }
 
        return true;
    }
}
