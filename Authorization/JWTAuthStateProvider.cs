using Authorization.DataTransferObjects;
using Authorization.Interfaces;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Authorization
{
    public class JWTAuthStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService _localStorageService;
        private readonly IAccessToken _accessTokenService;

        public JWTAuthStateProvider(ILocalStorageService localStorageService, IAccessToken accessTokenService)
        {
            _localStorageService = localStorageService;
            _accessTokenService = accessTokenService;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await _localStorageService.GetItemAsync<string>("UserHubToken");

            if (string.IsNullOrWhiteSpace(token))
            {
                return await Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())));
            }


            if (_accessTokenService.IsTokenValid(token) == true)
            {
                return await Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Role, _accessTokenService.GetRole(token)),
                    new Claim(ClaimTypes.Name, _accessTokenService.GetName(token)),
                    new Claim(ClaimTypes.Email, _accessTokenService.GetEmail(token))
                }, "ConnectedType"))));
            }

            return await Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())));
        }

        public void UserAuthenticated(UserDTO user)
        {
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email)
            }, "ConnectedType")))));
        }

        public async Task UserLogOut()
        {
            await _localStorageService.RemoveItemAsync("UserHubToken");
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()))));
        }

        public async Task<string> GetToken()
        {
            return await _localStorageService.GetItemAsync<string>("UserHubToken");
        }
    }
}
