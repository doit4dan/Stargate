using FluentValidation;
using Stargate.Server.Controllers;
using System.Net;

namespace Stargate.Server.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ValidationException ex) // Pass BadRequest for Validation Errors
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            var validationFailureResponse = new ValidationFailureResponse
            {
                Success = false,
                Message = "Validation Failure",
                ResponseCode = (int)HttpStatusCode.BadRequest,
                Errors = ex.Errors.Select(x => new ValidationResponse
                {
                    PropertyName = x.PropertyName,
                    Message = x.ErrorMessage
                })
            };

            await context.Response.WriteAsJsonAsync(validationFailureResponse);
        } 
        catch (BadHttpRequestException ex)
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

            await context.Response.WriteAsJsonAsync(new BaseResponse()
            {
                Message = ex.Message,
                Success = false,
                ResponseCode = (int)HttpStatusCode.BadRequest
            });
        }
        // Pass Internal Server Error for Remaining Errors ( Do not inform user of internal system details, e.g. Stack Trace )
        catch (Exception ex)
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            
            await context.Response.WriteAsJsonAsync(new BaseResponse()
            {
                Message = ex.Message,
                Success = false,
                ResponseCode = (int)HttpStatusCode.InternalServerError
            });
        }
    }
}
