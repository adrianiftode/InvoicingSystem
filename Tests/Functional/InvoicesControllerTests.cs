using Api.Models;
using System.Net;
using System.Threading.Tasks;
using Tests.Extensions.FluentAssertions;
using Tests.Functional.Extensions;
using Tests.Functional.Fixtures;
using Xunit;

namespace Tests.Functional
{
    public class InvoicesControllerTests : IClassFixture<InMemoryWebApplicationFactory>
    {
        private readonly InMemoryWebApplicationFactory _factory;

        public InvoicesControllerTests(InMemoryWebApplicationFactory factory)
        {
            _factory = factory;
        }

        [Theory]
        [InlineData("invoices", HttpStatusCode.NotFound)]
        [InlineData("invoices/1", HttpStatusCode.OK)]
        [InlineData("invoices/3", HttpStatusCode.NotFound)]
        [InlineData("invoices/string-like-value", HttpStatusCode.BadRequest)]
        public async Task Get_ShouldReturnExpectedResponse(string path, HttpStatusCode expectedStatusCode)
        {
            //Arrange
            var client = _factory.CreateClient().WithApiKey("admin123");

            //Act
            var response = await client.GetAsync(path);

            //Assert
            await response.Should().BeWithStatusCode(expectedStatusCode);
        }

        [Theory]
        [InlineData("invoices/1/notes", HttpStatusCode.OK)]
        [InlineData("invoices/2/notes", HttpStatusCode.OK)]
        [InlineData("invoices/3/notes", HttpStatusCode.NotFound)]
        public async Task GetNotes_ShouldReturnExpectedResponse(string path, HttpStatusCode expectedStatusCode)
        {
            //Arrange
            var client = _factory.CreateClient().WithApiKey("admin123");

            //Act
            var response = await client.GetAsync(path);

            //Assert
            await response.Should().BeWithStatusCode(expectedStatusCode);
        }

        [Fact]
        public async Task GetInvoiceResponse_WithValidId_ShouldReturnInvoiceModel()
        {
            //Arrange
            var client = _factory.CreateClient().WithApiKey("admin123");

            //Act
            var response = await client.GetAsync("/invoices/1");

            //Assert
            (await response.Should().BeOk<InvoiceModel>())
                .And.BeEquivalentTo(new
                {
                    Amount = 150.05m,
                    Identifier = "INV-001",
                    InvoiceId = 1
                });
        }
    }
}
