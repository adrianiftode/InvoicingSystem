using System.Net;
using System.Threading.Tasks;
using Tests.Extensions.FluentAssertions;
using Tests.Functional.Extensions;
using Tests.Functional.Fixtures;
using Xunit;

namespace Tests.Functional
{
    public class AuthorizationTests : IClassFixture<InMemoryWebApplicationFactory>
    {
        private readonly InMemoryWebApplicationFactory _factory;

        public AuthorizationTests(InMemoryWebApplicationFactory factory)
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
            await response.Should().BeWithStatusCode(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task WithRightSecret_ReturnsOk()
        {
            //Arrange
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Add("X-Api-Key", "user123");

            //Act
            var response = await client.GetAsync("/invoices/1");

            //Assert
            await response.Should().BeWithStatusCode(HttpStatusCode.OK);
        }

        [Fact]
        public async Task WithWrongSecret_ReturnsUnauthorized()
        {
            //Arrange
            var client = _factory.CreateClient("no-valid-user-key");

            //Act
            var response = await client.GetAsync("/invoices/1");

            //Assert
            await response.Should().BeWithStatusCode(HttpStatusCode.Unauthorized);
        }
    }
}
