using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenTelemetry;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace SystemWeb.OpenTelemetry.Application;

public class OpenTelemeteryApplication : HttpApplication
{
    protected static IServiceProvider RootServiceProvider { get; private set; }

    protected static readonly object ScopeKey = new();

    protected virtual void Application_Start()
    {
        var config = CreateConfigurationManager();
        ConfigureConfiguration(config);

        var services = CreateServiceCollection();

        services.AddSingleton(config);
        services.AddSingleton<IConfiguration>(config);

        services.AddLogging(logging =>
        {
            logging.Configure(options =>
            {
                options.ActivityTrackingOptions =
                    ActivityTrackingOptions.SpanId |
                    ActivityTrackingOptions.TraceId |
                    ActivityTrackingOptions.ParentId;
            });
            logging.AddOpenTelemetry(ot =>
            {
                ot.IncludeFormattedMessage = true;
                ot.IncludeScopes = true;
                ot.ParseStateValues = true;
            });
            ConfigureLogging(logging);
        });

        var otlp = services.AddOpenTelemetry()
            .ConfigureResource(ConfigureResource)
            .WithTracing(ConfigureTracing)
            .WithMetrics(ConfigureMetrics)
            .WithLogging(ConfigureLogging);

        System.Diagnostics.Debug.WriteLine("{0}={1}", "OTEL_EXPORTER_OTLP_PROTOCOL", config.GetValue<string>("OTEL_EXPORTER_OTLP_PROTOCOL"));
        System.Diagnostics.Debug.WriteLine("{0}={1}", "OTEL_EXPORTER_OTLP_ENDPOINT", config.GetValue<string>("OTEL_EXPORTER_OTLP_ENDPOINT"));
 
        if (config.GetValue<string>("OTEL_EXPORTER_OTLP_PROTOCOL", "http/protobuf") == "http/protobuf"
            && !string.IsNullOrEmpty(config.GetValue<string>("OTEL_EXPORTER_OTLP_ENDPOINT", "")))
        {
            otlp.UseOtlpExporter();
            System.Diagnostics.Debug.WriteLine("OTLP Exporter enabled");
        }
        else
        {
            System.Console.WriteLine("OTEL_EXPORTER_OTLP_PROTOCOL={0}", config.GetValue<string>("OTEL_EXPORTER_OTLP_PROTOCOL"));
            System.Console.WriteLine("OTEL_EXPORTER_OTLP_ENDPOINT={0}", config.GetValue<string>("OTEL_EXPORTER_OTLP_ENDPOINT"));
            System.Console.WriteLine("OTLP Exporter not enabled");
            otlp.WithLogging(l => l.AddConsoleExporter());
        }

        ConfigureServiceProvider(services);

        var serviceProvider = services.BuildServiceProvider();
        RootServiceProvider = serviceProvider;

        UseServiceProvider(serviceProvider);

        HostingEnvironment.RegisterObject(new OpenTelemetryRunner(serviceProvider));
    }

    protected virtual void ConfigureResource(ResourceBuilder builder) { }

    protected virtual void ConfigureLogging(ILoggingBuilder logging){ }

    protected virtual void ConfigureLogging(LoggerProviderBuilder logging) { }

    protected virtual void ConfigureMetrics(MeterProviderBuilder metrics)
    {
        metrics
            .AddAspNetInstrumentation()
            .AddRuntimeInstrumentation();
    }

    protected virtual void ConfigureTracing(TracerProviderBuilder tracing)
    {
        tracing
            .AddAspNetInstrumentation()
            .AddHttpClientInstrumentation();
    }

    protected virtual ServiceCollection CreateServiceCollection() => new();
    protected virtual ConfigurationManager CreateConfigurationManager() => new();

    protected virtual void ConfigureConfiguration(ConfigurationManager configuration)
    {
        configuration.AddEnvironmentVariables();
    }

    protected virtual void ConfigureServiceProvider(ServiceCollection services) { }

    protected virtual void UseServiceProvider(ServiceProvider serviceProvider) { }

    protected virtual void Application_BeginRequest(object sender, EventArgs e) { }

    protected virtual void Application_EndRequest(object sender, EventArgs e) { }
}
