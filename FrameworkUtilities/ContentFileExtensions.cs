using Microsoft.AspNetCore.SystemWebAdapters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using System.Web.Hosting;

namespace System.Web;

public static class ContentFileExtensions
{
    public static ISystemWebAdapterBuilder AddVirtualizedContentDirectories(this ISystemWebAdapterBuilder builder)
    {
        if (GetProvider() is { } provider)
        {
            HostingEnvironment.RegisterVirtualPathProvider(new FileProviderVirtualPathProvider(provider));

            builder.Services.AddSingleton<IHttpModule>(new StaticFileProviderHttpModule(provider));
        }

        return builder;
    }

    private static CompositeFileProvider? GetProvider()
    {
        var binDir = Path.Combine(HttpRuntime.BinDirectory, "contentDirectories.txt");

        if (File.Exists(binDir))
        {
            var providers = File.ReadAllLines(binDir).Where(Directory.Exists).Select(line => new PhysicalFileProvider(line)).ToList();
            if (providers.Count != 0)
            {
                return new CompositeFileProvider(providers);
            }
        }

        return null;
    }
}
