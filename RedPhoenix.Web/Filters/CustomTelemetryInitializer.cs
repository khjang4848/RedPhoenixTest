namespace RedPhoenix.Web.Filters;

using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;

public class CustomTelemetryInitializer : ITelemetryInitializer
{
    private const string UserIdKey = "UserId";

    public void Initialize(ITelemetry telemetry)
    {
        if (telemetry is not RequestTelemetry requestTelemetry) return;

        var props = requestTelemetry.Properties;

        props.TryAdd(UserIdKey, UserIdKey);
    }
}

