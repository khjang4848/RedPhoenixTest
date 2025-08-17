namespace RedPhoenix.Web.Filters;

using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Net;

using Messages;


public class ClientIpCheckActionFilter(string? safeList, ILogger logger) 
    : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var remoteIp = context.HttpContext.Connection.RemoteIpAddress;
        logger.LogDebug("Remote IpAddress: {RemoteIp}", remoteIp);
        var ip = safeList?.Split(';');

        if (remoteIp!.IsIPv4MappedToIPv6)
        {
            remoteIp = remoteIp.MapToIPv4();
        }

        var badIp = ip != null && !ip.Select(IPAddress.Parse).Contains(remoteIp);

        if (badIp)
        {
            logger.LogWarning("Forbidden Request from IP: {RemoteIp}",
                remoteIp);
            context.Result = new JsonResult(new Error
            {
                Name = "404 Error",
                Message = $"Forbidden Request from IP: {remoteIp}"

            })
            {
                StatusCode = (int)HttpStatusCode.Forbidden
            };

            return;
        }

        base.OnActionExecuting(context);
    }
}

