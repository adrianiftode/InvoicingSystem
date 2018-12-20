using FluentAssertions;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Tests.Functional.Extensions;
using Tests.Functional.Fixtures;
using Xunit;

namespace Tests.Functional
{
    public class NotesControllerTests : IClassFixture<InMemoryWebApplicationFactory>
    {
        private readonly InMemoryWebApplicationFactory _factory;

        public NotesControllerTests(InMemoryWebApplicationFactory factory)
        {
            _factory = factory;
        }

        [Theory]
        [InlineData("notes", HttpStatusCode.NotFound)]
        [InlineData("notes/1", HttpStatusCode.OK)]
        [InlineData("notes/333", HttpStatusCode.NotFound)]
        [InlineData("notes/string-like-value", HttpStatusCode.BadRequest)]
        public async Task Get_ShouldReturnExpectedResponse(string path, HttpStatusCode expectedStatusCode)
        {
            //Arrange
            var client = _factory.CreateClient("admin123");

            //Act
            var response = await client.GetAsync(path);

            //Assert
            response.StatusCode.Should().Be(expectedStatusCode, await response.Content.ReadAsStringAsync());
        }

        [Fact]
        public async Task Put_WithDifferentUserThanTheOneThatCreatedTheNote_ReturnsForbidden()
        {
            //Arrange
            var client = _factory.CreateClient("admin345");

            //Act
            var response = await client.PutAsJsonAsync("/notes", new
            {
                noteId = 1,
                text = "Text"
            });

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }
    }
}
