using Stargate.Server.Controllers;

namespace Stargate.Server.Middleware;

public class ValidationError : Error
{
    public required IEnumerable<ValidationResponse> Errors { get; init; }
}