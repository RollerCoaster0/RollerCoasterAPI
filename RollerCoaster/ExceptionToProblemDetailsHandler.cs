using System.Net;
using Microsoft.AspNetCore.Mvc;
using RollerCoaster.Services.Abstractions.Common;

namespace RollerCoaster;

//

public class ExceptionToProblemDetailsHandler(ILogger<ExceptionToProblemDetailsHandler> logger):
    Microsoft.AspNetCore.Diagnostics.IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is NotFoundError notFoundErrorException)
            await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
            {
                Title = notFoundErrorException.Message,
                Status = (int)HttpStatusCode.NotFound
            }, cancellationToken: cancellationToken);
        
        else if (exception is AccessDeniedError accessDeniedErrorException)
            await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
            {
                Title = accessDeniedErrorException.Message,
                Status = (int)HttpStatusCode.Forbidden
            }, cancellationToken: cancellationToken);
        
        else if (exception is ProvidedDataIsInvalidError providedDataIsInvalidErrorException)
            await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
            {
                Title = providedDataIsInvalidErrorException.Message,
                Status = (int)HttpStatusCode.Forbidden
            }, cancellationToken: cancellationToken);

        else
        {
            logger.LogError(exception, "Произошла неизвестная ошибка");
            
            await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
            {
                Title = "Произошла неизвестная ошибка. Попробуйте позже",
                Status = (int)HttpStatusCode.InternalServerError
            }, cancellationToken: cancellationToken);
        }
 
        return true;
    }
}
