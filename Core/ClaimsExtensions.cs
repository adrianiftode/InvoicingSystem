using System.Security.Claims;

namespace Core
{
    public static class ClaimsExtensions
    {
        public static bool IsAdmin(this ClaimsPrincipal claimsPrincipal)
            => claimsPrincipal?.IsInRole("Admin") ?? false;
        public static bool IsUser(this ClaimsPrincipal claimsPrincipal)
            => claimsPrincipal?.IsInRole("User") ?? false;

        public static string GetIdentity(this ClaimsPrincipal claimsPrincipal)
            => claimsPrincipal?.Identity.Name;
    }
}
