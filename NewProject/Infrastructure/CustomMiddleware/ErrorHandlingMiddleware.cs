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
                case ItemNotFoundException itemNotFoundEx:
                    httpContext.Response.StatusCode = StatusCodes.Status404NotFound;
                    await httpContext.Response.WriteAsync(itemNotFoundEx.Message);
                    break;
                case UnAuthorizeException unAuthorizeExceptionEx:
                    httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await httpContext.Response.WriteAsync(unAuthorizeExceptionEx.Message);
                    break;
                case TooManyRequestException tooManyRequestEx:
                    httpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                    await httpContext.Response.WriteAsync(tooManyRequestEx.Message);
                    break;
                case VerifyActionFailedException verifyActionEx:
                    httpContext.Response.StatusCode = StatusCodes.Status422UnprocessableEntity;
                    await httpContext.Response.WriteAsync(verifyActionEx.Message);
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