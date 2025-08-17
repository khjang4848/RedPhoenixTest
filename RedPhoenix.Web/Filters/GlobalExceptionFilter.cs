namespace RedPhoenix.Web.Filters;

using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Net;

using Exceptions;
using Messages;

public class GlobalExceptionFilter : IExceptionFilter, IDisposable
{
    private readonly ILogger _logger;
    private bool _disposed;

    public GlobalExceptionFilter(ILoggerFactory logger)
    {
        if (logger == null)
        {
            throw new ArgumentNullException(nameof(logger));
        }

        _logger = logger.CreateLogger("Exception Filter");
    }


    public void OnException(ExceptionContext context)
    {
        var response = new ErrorResponse("500", context.Exception.Message,
            context.Exception.StackTrace);

        context.Result = new JsonResult(response)
        {
            StatusCode = GetHttpStatusCode(context.Exception),
        };

        _logger.LogError("GlobalExceptionFilter Message : {message1} \n " +
                         "StackTrace : {message2}",
            context.Exception.Message, context.Exception.StackTrace);
    }

    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }
        _disposed = true;
    }

    private static int GetHttpStatusCode(Exception ex)
    {
        if (ex is HttpResponseException exception)
        {
            return (int)exception.HttpStatusCode;
        }

        return (int)HttpStatusCode.InternalServerError;
    }
}

