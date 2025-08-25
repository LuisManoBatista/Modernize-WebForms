using System.Configuration;
using System.Web;

namespace WebFormsApp
{
    public static class SystemWebAdapterConfig 
    {
        public static void Register(this HttpApplication application)
        {
            SystemWebAdapterConfiguration.AddSystemWebAdapters(application)
               .AddVirtualizedContentDirectories()
               .AddProxySupport(options => options.UseForwardedHeaders = true)
               .AddJsonSessionSerializer(options =>
               {
                   options.RegisterKey<string>("test-value");
               })
               .AddRemoteAppServer(options =>
               {
                   options.ApiKey = ConfigurationManager.AppSettings["RemoteAppApiKey"];
               })
               .AddSessionServer();
        }
    }
}
