using AdvanceChat.Data;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace AdvanceChat.Client.Data
{

    //class for logut 
    internal static class IdentityComponentEndpointRouteBuilderExtension
    {
        public static IEndpointConventionBuilder endpointConventionBuilder(this IEndpointRouteBuilder endpointRoute)
        {
            var group = endpointRoute.MapGroup("/");
            group.MapPost("/logout", async(ClaimsPrincipal user, SignInManager<AppUser> signInManager) =>
            {
                await signInManager.SignOutAsync();
                return TypedResults.LocalRedirect("/login");
            });

            return group;
        }
    }
}
