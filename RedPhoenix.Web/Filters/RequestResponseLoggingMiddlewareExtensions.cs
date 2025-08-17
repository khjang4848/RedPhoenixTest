namespace RedPhoenix.Web.Filters;

public static class RequestResponseLoggingMiddlewareExtensions
{
    public static IApplicationBuilder UseRequestResponseLogging(
        this IApplicationBuilder builder)
        => builder.UseMiddleware<RequestResponseLoggingMiddleware>();
}

