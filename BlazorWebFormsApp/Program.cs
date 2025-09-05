using BlazorWebFormsApp;
using BlazorWebFormsApp.Components;
using BlazorWebFormsApp.Components.Components;
using BlazorWebFormsApp.Providers;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.DataProtection;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityAuthenticationStateProvider>();

builder.Services.AddHttpForwarder();

var sharedApplicationName = "SharedCookieWebFormApp";
builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(Path.Combine(Path.GetTempPath(), "sharedkeys", sharedApplicationName)))
    .SetApplicationName(sharedApplicationName);

builder.Services.AddAuthentication()
    .AddCookie("Identity.Application", options => options.Cookie.Name = ".AspNet.ApplicationCookie");

builder.Services.AddRouting(options => options.ConstraintMap.Add("isAxdFile", typeof(AxdConstraint)));

builder.Services.AddSystemWebAdapters()
    .AddJsonSessionSerializer(options =>
    {
        options.RegisterKey<string>(SystemWebAdapterConfig.SessionValue);
    })
    .AddRemoteAppClient(options =>
    {
        options.RemoteAppUrl = new(builder.Configuration[SystemWebAdapterConfig.ProxyTo]!);
        options.ApiKey = builder.Configuration[SystemWebAdapterConfig.RemoteAppApiKey]!;
    })
    .AddSessionClient();


// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents(options => options.RootComponents.RegisterCustomElement<HelloWorld>("hello-world"));

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthorization();
app.UseAntiforgery();

app.MapStaticAssets();

/*
 * When Blazor interactive SSR establishes a SignalR connection, a CONNECT method is requested.
 * This request will stay open until Blazor sends a disconnect request. 
 * Since it stays open, the session is not properly committed and will stay in the heartbeat loop on the framework side of System.WebAdapters.
 * We can either:
 *     A: Block all connect requests from using System.Web.Adapters
 *     B: Block all requests from paths containing "/_blazor"
 * TLDR: We need to ensure that SystemWeb.Adapters is not used with Blazor SignalR
 */
app.UseWhen(
    (context) => !HttpMethods.IsConnect(context.Request.Method),
    appBuilder => appBuilder.UseSystemWebAdapters());

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .RequireSystemWebAdapterSession();

app.MapForwarder("/{**catch-all}", app.Configuration[SystemWebAdapterConfig.ProxyTo]!).Add(static builder => ((RouteEndpointBuilder)builder).Order = int.MaxValue);

await app.RunAsync();
