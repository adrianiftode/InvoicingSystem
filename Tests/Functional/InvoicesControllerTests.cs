using FluentAssertions;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Api.Models;
using Tests.Fixtures;
using Xunit;
using Xunit.Abstractions;

namespace Tests.Functional
{
    public class InvoicesControllerTests : IClassFixture<InMemoryWebApplicationFactory>
    {
        private readonly InMemoryWebApplicationFactory _factory;
        private readonly ITestOutputHelper _output;

        public InvoicesControllerTests(InMemoryWebApplicationFactory factory, ITestOutputHelper output)
        {
            _factory = factory;
            _output = output;
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
            var content = await response.Content.ReadAsStringAsync();
            _output.WriteLine(content);
            response.StatusCode.Should().Be(expectedStatusCode);
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
            var content = await response.Content.ReadAsStringAsync();
            _output.WriteLine(content);
            response.StatusCode.Should().Be(expectedStatusCode);
        }

        [Fact]
        public async Task GetInvoiceResponse_WithValidId_ShouldReturnInvoiceModel()
        {
            //Arrange
            var client = _factory.CreateClient().WithApiKey("admin123");

            //Act
            var response = await client.GetAsync("/invoices/1");

            //Assert
            var invoice = await response.Content.ReadAsAsync<InvoiceModel>();
            invoice.Amount.Should().Be(150.05m);
            invoice.Identifier.Should().Be("INV-001");
            invoice.InvoiceId.Should().Be(1);
            invoice.Notes.Should().NotBeNullOrEmpty();
        }
    }
}
