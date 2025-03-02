namespace Stargate.Server.Controllers;

public class ErrorResponse : BaseResponse
{
    public ErrorResponse()
    {
        Success = false;
    }
    public IEnumerable<ValidationResponse> ValidationErrors { get; init; } = Enumerable.Empty<ValidationResponse>();
}