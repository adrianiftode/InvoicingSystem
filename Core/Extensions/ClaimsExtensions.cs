using System.Security.Claims;

namespace Core
{
    internal static class ClaimsExtensions
    {
        public static bool IsAdmin(this ClaimsPrincipal claimsPrincipal)
            => claimsPrincipal?.IsInRole(Roles.Admin) ?? false;
        public static bool IsUser(this ClaimsPrincipal claimsPrincipal)
            => claimsPrincipal?.IsInRole(Roles.User) ?? false;

        public static string GetIdentity(this ClaimsPrincipal claimsPrincipal)
            => claimsPrincipal?.Identity.Name;
    }
}
