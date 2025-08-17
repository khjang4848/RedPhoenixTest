namespace RedPhoenix.Web.Messages;

public class RequestResponseData
{
    
    /// <summary>
    /// 
    /// </summary>
    public DateTime? RequestTimestamp { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string RequestUri { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    public string RequestMethod { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    public string RequestBody { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    public string IpAddress { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    public string RequestHeaders { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    public string RequestContentType { get; set; } = string.Empty;
    /// <summary>
    /// 
    /// </summary>
    public DateTime? ResponseTimestamp { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string ResponseBody { get; set; } = string.Empty;
    /// <summary>
    /// 
    /// </summary>
    public int ResponseStatusCode { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string Machine { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    public string? User { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        string separator = string.Concat(Enumerable.Repeat("*", 50));
        var lb = "\r\n";
        return $"{lb}{separator}{lb}" +
                $"Http Request Information:{lb}" +
                $"Machine:                  {Machine}{lb}" +
                $"User:                     {User}{lb}" +
                $"Request Timestamp:        {RequestTimestamp}{lb}" +
                $"Request IpAddress:        {IpAddress}{lb}" +
                $"Request Uri:              {RequestUri}{lb}" +
                $"Request Method:           {RequestMethod}{lb}" +
                $"RequestHeaders:           {RequestHeaders}{lb}" +
                $"Request ContentType:      {RequestContentType}{lb}" +
                $"Request ContentBody:      {RequestBody}{lb}" +
                $"RequestHeaders:           {RequestHeaders}{lb}" +
                $"{Environment.NewLine}Http Response Information:{Environment.NewLine}" +
                $"ResponseStatusCode:   {ResponseStatusCode}{lb}" +
                $"ResponseContentBody:  {ResponseBody}{lb}" +
                $"{lb}{separator}{lb}{Environment.NewLine}";
    }
}
