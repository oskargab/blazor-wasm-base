using BlazorApp1.Shared;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net;
using System.Security.Claims;

namespace BlazorApp1.Client.Providers
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {

        private ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity());
        private readonly HttpClient httpClient;
        public CustomAuthStateProvider(HttpClient http)
        {
            httpClient = http;
        }
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            return new AuthenticationState(claimsPrincipal);
        }

        public void SetAuthInfo(UserProfileDto userProfile)
        {
            var identity = new ClaimsIdentity(new[]{
            new Claim(ClaimTypes.Email, userProfile.Email),
            new Claim(ClaimTypes.Name, $"{userProfile.FirstName} {userProfile.LastName}")
        }, "AuthCookie");

            claimsPrincipal = new ClaimsPrincipal(identity);
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public void ClearAuthInfo()
        {
            claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity());
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
    }
}

