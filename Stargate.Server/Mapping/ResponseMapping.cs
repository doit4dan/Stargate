using Stargate.Server.Controllers;
using Stargate.Server.Middleware;

namespace Stargate.Server.Mapping;

public static class ResponseMapping
{
    public static ErrorResponse MapToResponse(this Error error)
    {
        return new ErrorResponse
        {            
            ResponseCode = (int)error.StatusCode,
            Message = error.ShowMessageToConsumer ? error.Message : "An error has occured within the system. Please try again.."
        };
    }

    public static ErrorResponse MapToResponse(this ValidationError error)
    {
        return new ErrorResponse
        {            
            ResponseCode = (int)error.StatusCode,
            Message = error.ShowMessageToConsumer ? error.Message : "An error has occured within the system. Please try again..",
            ValidationErrors = error.Errors
        };
    }
}
