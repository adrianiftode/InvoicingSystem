using System.Security.Claims;
using System.Security.Principal;

namespace Tests.Core
{
    internal static class TestsHelpers
    {
        public static ClaimsPrincipal CreateUser(string user, string role) => new ClaimsPrincipal(new GenericPrincipal(new GenericIdentity(user, "Api User"), new[] { role }));
    }
}