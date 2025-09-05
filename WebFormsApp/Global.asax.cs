using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using SystemWeb.OpenTelemetry.Application;

namespace WebFormsApp
{
    public class Global : OpenTelemeteryApplication
    {
        protected override void Application_Start()
        {
            base.Application_Start();

            // Code that runs on application startup
            SystemWebAdapterConfig.Register(this);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected override void Application_BeginRequest(object sender, EventArgs e)
        {
            var scope = RootServiceProvider.CreateScope();
            HttpContext.Current.Items[ScopeKey] = scope;
        }

        protected override void Application_EndRequest(object sender, EventArgs e)
        {
            // Dispose the scope at the end of the request
            if (HttpContext.Current.Items[ScopeKey] is IServiceScope scope)
            {
                scope.Dispose();
            }
        }

        public static IServiceProvider GetRequestServiceProvider()
        {
            if (HttpContext.Current.Items[ScopeKey] is IServiceScope scope)
            {
                return scope.ServiceProvider;
            }
            return RootServiceProvider;
        }

        protected override void ConfigureConfiguration(ConfigurationManager configuration)
        {
            base.ConfigureConfiguration(configuration);

            var appSettings = System.Configuration.ConfigurationManager.AppSettings;
            var dict = appSettings.AllKeys.ToDictionary(k => k, k => appSettings[k]);
            configuration.AddInMemoryCollection(dict);
        }
    }
}