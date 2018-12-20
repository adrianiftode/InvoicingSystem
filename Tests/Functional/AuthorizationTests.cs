using System.Net;
using System.Threading.Tasks;
using Api;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Tests.Functional.Extensions;
using Xunit;

namespace Tests.Functional
{
    public class AuthorizationTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;

        public AuthorizationTests(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task WithoutXApiKey_ReturnsUnauthorized()
        {
            //Arrange
            var client = _factory.CreateClient();

            //Act
            var response = await client.GetAsync("/invoices/1");

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task WithRightSecret_ReturnsOkAndIdentityInfo()
        {
            //Arrange
            var client = _factory.CreateClient("user123");
            
            //Act
            var response = await client.GetAsync("/invoices/1");

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task WithWrongSecret_ReturnsUnauthorized()
        {
            //Arrange
            var client = _factory.CreateClient("no-valid-user-key");

            //Act
            var response = await client.GetAsync("/invoices/1");

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }
    }
}
