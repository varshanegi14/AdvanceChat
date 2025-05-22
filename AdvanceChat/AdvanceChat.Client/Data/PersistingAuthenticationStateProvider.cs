using ChatModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace AdvanceChat.Client.Data
{

    //this class used to know about state of user in client 
    public class PersistingAuthenticationStateProvider : AuthenticationStateProvider
    {
        private static readonly Task<AuthenticationState> defaultauthenticationState =
            Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())));

        private readonly Task<AuthenticationState> authenticationStateTask = defaultauthenticationState;

        public PersistingAuthenticationStateProvider(PersistentComponentState state)
        {
            if (!state.TryTakeFromJson<UserInfo>(nameof(UserInfo), out var userInfo) || userInfo is null)
                return;

            Claim[] claims = [
                new Claim(ClaimTypes.NameIdentifier,userInfo.UserId!),
                 new Claim(ClaimTypes.Email,userInfo.Email!),
                  new Claim(ClaimTypes.Name,userInfo.Fullname!)
                ];
            authenticationStateTask = Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(claims, nameof(PersistingAuthenticationStateProvider)))));
        }
        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        => authenticationStateTask;
    }
}
