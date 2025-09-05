using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;
using System.Web.Hosting;

namespace SystemWeb.OpenTelemetry.Application;

internal class OpenTelemetryRunner : IRegisteredObject
{
    private readonly ServiceProvider serviceProvider;

    public OpenTelemetryRunner(ServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
        _ = serviceProvider.GetService<MeterProvider>();
        _ = serviceProvider.GetService<TracerProvider>();
        _ = serviceProvider.GetService<LoggerProvider>();
        var logger = serviceProvider.GetService<ILogger<OpenTelemetryRunner>>();
        logger.LogInformation("Your log message");

    }

    public void Stop(bool immediate)
    {
        serviceProvider.Dispose();
        HostingEnvironment.UnregisterObject(this);
    }
}
