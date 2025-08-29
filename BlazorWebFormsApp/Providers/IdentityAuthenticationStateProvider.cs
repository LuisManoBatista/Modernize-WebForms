using Microsoft.AspNetCore.Components.Authorization;

namespace BlazorWebFormsApp.Providers;

public class IdentityAuthenticationStateProvider(IHttpContextAccessor httpContextAccessor) : AuthenticationStateProvider
{
    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var user = httpContextAccessor.HttpContext?.User ?? new System.Security.Claims.ClaimsPrincipal();
        return Task.FromResult(new AuthenticationState(user));
    }
}
