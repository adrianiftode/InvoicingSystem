using System.Security.Claims;
using System.Security.Principal;
using Core;
using FluentAssertions;

namespace Tests.Core
{
    internal static class TestsHelpers
    {
        public static ClaimsPrincipal CreateUser(string user, string role) => new ClaimsPrincipal(new GenericPrincipal(new GenericIdentity(user, "Api User"), new[] { role }));

        public static void ShouldBeSuccess<TItem>(this Result<TItem> result)
            => result.Status.Should().Be(ResultStatus.Success); // not really the fluent assertions style

        public static void ShouldFail<TItem>(this Result<TItem> result)
            => result.Status.Should().NotBe(ResultStatus.Success);
    }
}