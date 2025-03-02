using FluentValidation;
using Stargate.Server.Controllers;
using Stargate.Server.Mapping;
using System.Net;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Stargate.Server.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ValidationException ex) // Pass BadRequest for Validation Errors
        {
            await HandleValidationExceptionAsync(context, ex);
        } 
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {              
        var error = new Error
        {
            Message = exception.Message,
            ExceptionType = exception.GetType().Name,
            StatusCode = exception switch
            {
                BadHttpRequestException => HttpStatusCode.BadRequest,                
                _ => HttpStatusCode.InternalServerError
            },
            ShowMessageToConsumer = exception switch
            {
                BadHttpRequestException => true,
                _ => false
            },
            StackTrace = exception.StackTrace
        };

        _logger.LogError(error.ToString());

        context.Response.StatusCode = (int)error.StatusCode;
        var response = error.MapToResponse();
        await context.Response.WriteAsJsonAsync(response);
    }

    private async Task HandleValidationExceptionAsync(HttpContext context, ValidationException exception)
    {
        var error = new ValidationError
        {
            Message = exception.Message,
            ExceptionType = exception.GetType().Name,
            StatusCode = HttpStatusCode.BadRequest,
            StackTrace = exception.StackTrace,
            ShowMessageToConsumer = true,
            Errors = exception.Errors.Select(x => new ValidationResponse
            {
                PropertyName = x.PropertyName,
                Message = x.ErrorMessage
            })
        };

        _logger.LogError(error.ToString());

        context.Response.StatusCode = (int)error.StatusCode;
        var response = error.MapToResponse();
        await context.Response.WriteAsJsonAsync(response);
    }
}
