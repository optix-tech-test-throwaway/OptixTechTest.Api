using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics;

namespace OptixTechTest.Api.Middleware;

/// <summary>
/// Handles global exceptions across the application by implementing <see cref="IExceptionHandler"/>.
/// </summary>
/// <remarks>
/// This handler logs exceptions and returns appropriate HTTP responses based on the exception type:
/// <list type="bullet">
///     <item><description>For validation-related exceptions, returns a 400 Bad Request with validation details</description></item>
///     <item><description>For all other exceptions, returns a 500 Internal Server Error with exception details</description></item>
/// </list>
/// </remarks>
public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext context, 
        Exception exception, 
        CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "An unhandled exception occurred");

        switch (exception)
        {
            case JsonException:
            case BadHttpRequestException:
            case ValidationException:
                await TypedResults
                    .ValidationProblem(
                        errors: new Dictionary<string, string[]> 
                        { 
                            { "body", [exception.Message]} 
                        },
                        title: "Validation Error",
                        type: "https://tools.ietf.org/html/rfc7231#section-6.5.1"
                    )
                    .ExecuteAsync(context);
                break;
            default:
                await TypedResults
                    .Problem(
                        statusCode: StatusCodes.Status500InternalServerError,
                        title: exception.GetType().Name,
                        detail: exception.Message)
                    .ExecuteAsync(context);
                break;
        }

        return true;
    }
}
