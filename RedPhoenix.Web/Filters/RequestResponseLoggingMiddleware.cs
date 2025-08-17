namespace RedPhoenix.Web.Filters;

using Microsoft.IO;
using Messages;
using System.Text;
using Microsoft.ApplicationInsights.DataContracts;

public class RequestResponseLoggingMiddleware(RequestDelegate next, 
    ILoggerFactory loggerFactory)
{
    private readonly ILogger _logger = loggerFactory
        .CreateLogger<RequestResponseLoggingMiddleware>();
    private readonly RecyclableMemoryStreamManager _recyclableMemoryStreamManager = new();
    private RequestResponseData Log { get; set; } = new();

    public async Task Invoke(HttpContext context)
    {
        //send to log if it real api call, ignore home page and swagger store only post put and delete
        if (context.Request.Method != "GET")
        {
            if (context.User.Identity != null)
                Log = new RequestResponseData()
                {
                    User = context.User.Identity.Name
                };

            await LogRequest(context);
            await LogResponse(context);
            var logResult = Log.ToString();

            //Todo Logstash 호출되도록 변경
            var requestTelemetry = context.Features.Get<RequestTelemetry>();
            requestTelemetry!.Properties.Add("UserId", "SYSTEM");
            requestTelemetry.Properties.Add("Application", "Web API");


            _logger.LogInformation(logResult);

        }
        else
        {
            await next(context);
        }
    }

    private async Task LogRequest(HttpContext context)
    {
        try
        {

            var headers = String.Empty;
            var requestBodyStream = new MemoryStream();

            headers = context.Request.Headers.Keys
                .Aggregate(
                    headers, (current, key) => current +
                      (key + "=" + context.Request.Headers[key] + Environment.NewLine));

            context.Request.EnableBuffering();

            await using var requestStream = _recyclableMemoryStreamManager.GetStream();
            await context.Request.Body.CopyToAsync(requestStream);

            Log.RequestTimestamp = DateTime.Now;
            Log.RequestUri = $"{context.Request.Scheme}://{context.Request.Host}" +
                             $"{context.Request.Path}{context.Request.QueryString}";
            Log.IpAddress = $"{context.Request.Host.Host}";
            Log.RequestMethod = $"{context.Request.Method}";
            Log.Machine = Environment.MachineName;
            Log.RequestContentType = context.Request.ContentType;
            Log.RequestBody = ReadStreamInChunks(requestStream);
            //log.RequestBody = FormatRequest(context.Request);
            Log.RequestHeaders = headers;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "request/response logging exception");
            throw;
        }
        context.Request.Body.Position = 0;
    }

    private async Task LogResponse(HttpContext context)
    {
        var originalBodyStream = context.Response.Body;

        await using var responseBody = _recyclableMemoryStreamManager.GetStream();
        context.Response.Body = responseBody;

        await next(context);

        Log.ResponseTimestamp = DateTime.Now;
        Log.ResponseBody = await FormatResponse(context.Response);
        Log.ResponseStatusCode = context.Response.StatusCode;

        await responseBody.CopyToAsync(originalBodyStream);
    }

    private static string ReadStreamInChunks(Stream stream)
    {
        const int readChunkBufferLength = 4096;
        stream.Seek(0, SeekOrigin.Begin);
        using var textWriter = new StringWriter();
        using var reader = new StreamReader(stream);
        var readChunk = new char[readChunkBufferLength];
        int readChunkLength;
        do
        {
            readChunkLength = reader.ReadBlock(readChunk, 0, readChunkBufferLength);
            textWriter.Write(readChunk, 0, readChunkLength);
        } while (readChunkLength > 0);
        return textWriter.ToString();
    }

    private string FormatRequest(HttpRequest request)
    {
        var body = request.Body;
        //request.EnableRewind();

        var buffer = new byte[Convert.ToInt32(request.ContentLength)];
        request.Body.ReadAsync(buffer, 0, buffer.Length);
        var bodyAsText = Encoding.UTF8.GetString(buffer);
        //request.Body = body;

        return $"{request.QueryString} {bodyAsText}";
    }

    private async Task<string> FormatResponse(HttpResponse response)
    {
        response.Body.Seek(0, SeekOrigin.Begin);
        var text = await new StreamReader(response.Body).ReadToEndAsync();
        response.Body.Seek(0, SeekOrigin.Begin);

        return $"{text}";
    }
}

