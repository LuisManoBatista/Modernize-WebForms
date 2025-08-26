using System.Configuration;
using System.Web;

namespace WebFormsApp
{
    public static class SystemWebAdapterConfig 
    {
        public const string RemoteAppApiKey = "RemoteAppApiKey";
        
        public const string SessionValue = "session-value";

        public static void Register(this HttpApplication application)
        {
            SystemWebAdapterConfiguration.AddSystemWebAdapters(application)
               .AddVirtualizedContentDirectories()
               .AddProxySupport(options => options.UseForwardedHeaders = true)
               .AddJsonSessionSerializer(options =>
               {
                     options.RegisterKey<string>(SessionValue);
               })
               .AddRemoteAppServer(options =>
               {
                   options.ApiKey = ConfigurationManager.AppSettings[RemoteAppApiKey];
               })
               .AddSessionServer();
        }
    }
}
