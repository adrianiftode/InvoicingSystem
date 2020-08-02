using Core;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Api.Authentication
{
    public static class UserApiKeyDefaults
    {
        public const string AuthenticationScheme = "User Key";
    }
    public class CustomAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly Dictionary<string, ClaimsPrincipal> _users;
        public CustomAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
            => _users = new Dictionary<string, ClaimsPrincipal>
            {
                {"admin123", CreateFrom("1", Roles.Admin)},
                {"admin345", CreateFrom("2", Roles.Admin)},
                {"user123", CreateFrom("3", Roles.User)},
                {"user345", CreateFrom("4", Roles.User)},
            };

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var apiKey = Context.Request.Headers["X-Api-Key"];
            if (!string.IsNullOrEmpty(apiKey) && _users.TryGetValue(apiKey, out var identity))
            {
                var authenticationTicket = new AuthenticationTicket(identity,
                    new AuthenticationProperties(),
                    UserApiKeyDefaults.AuthenticationScheme);
                return Task.FromResult(AuthenticateResult.Success(authenticationTicket));
            }


            return Task.FromResult(AuthenticateResult.Fail("ApiKey is not present or user is not authenticated."));
        }

        private static ClaimsPrincipal CreateFrom(string identity, string role)
            => new ClaimsPrincipal(new GenericPrincipal(new GenericIdentity(identity, "Api User"), new[] { role }));
    }
}
