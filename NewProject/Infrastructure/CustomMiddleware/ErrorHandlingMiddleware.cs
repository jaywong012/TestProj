using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Domain.ErrorHandlingManagement;

namespace Infrastructure.CustomMiddleware;

public class ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await next(httpContext);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An unhandled exception has occurred.");

            switch (ex)
            {
                case ItemNotFoundException:
                    httpContext.Response.StatusCode = StatusCodes.Status404NotFound;
                    await httpContext.Response.WriteAsync("Resource not found");
                    break;
                case UnAuthorizeException:
                    httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await httpContext.Response.WriteAsync("Unable to authorize your account");
                    break;
                case InvalidOperationException:
                    httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                    await httpContext.Response.WriteAsync("Bad request");
                    break;
                default:
                    httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    await httpContext.Response.WriteAsync("An unexpected error occurred");
                    break;
            }
        }
    }
}

public static class ErrorHandlingMiddlewareExtensions
{
    public static void UseErrorHandling(this IApplicationBuilder builder)
    {
        builder.UseMiddleware<ErrorHandlingMiddleware>();
    }
}