using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
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
            var client = _factory.CreateClient();

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
            var client = _factory.CreateClient();

            //Act
            var response = await client.GetAsync(path);

            //Assert
            var content = await response.Content.ReadAsStringAsync();
            _output.WriteLine(content);
            response.StatusCode.Should().Be(expectedStatusCode);
        }
    }
}
