using Api;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using Xunit.Abstractions;

namespace FunctionalTests
{
    public class ValuesControllerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;
        private readonly ITestOutputHelper _output;

        public ValuesControllerTests(WebApplicationFactory<Startup> factory, ITestOutputHelper output)
        {
            _factory = factory;
            _output = output;
        }

        [Theory]
        [InlineData("api/values", HttpStatusCode.OK)]
        [InlineData("api/values/", HttpStatusCode.OK)]
        [InlineData("api/values/1", HttpStatusCode.OK)]
        [InlineData("api/values/string-like-value", HttpStatusCode.BadRequest)]
        [InlineData("api/not-found", HttpStatusCode.NotFound)]
        [InlineData("not-found", HttpStatusCode.NotFound)]
        public async Task Get_ShouldReturnExpectedResponse(string path, HttpStatusCode expectedStatusCode)
        {
            //Arrange
            var client = _factory.CreateClient();

            //Act
            var response = await client.GetAsync(path);


            //Assert
            var content = await response.Content.ReadAsStringAsync();
            _output.WriteLine(content);
            Assert.Equal(expectedStatusCode, response.StatusCode);
        }

        [Fact]
        public async Task Post_ShouldReturnExpectedResponse()
        {
            //Arrange
            var client = _factory.CreateClient();

            //Act
            var response = await client.PostAsJsonAsync("api/values/1", "value");


            //Assert
            var content = await response.Content.ReadAsStringAsync();
            _output.WriteLine(content);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Post_WithLongerTextValue_ShouldReturnBadRequest()
        {
            //Arrange
            var client = _factory.CreateClient();

            //Act
            var response = await client.PostAsJsonAsync("api/values/1", "big value big value big value");

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            dynamic content = await response.Content.ReadAsAsync<dynamic>();
            Assert.Equal("The value is too big", content.value.ToObject<string[]>()[0]);
        }

        [Fact]
        public async Task Post_WhenBooValue_ShouldRespondWith500()
        {
            //Arrange
            var client = _factory.CreateClient();

            //Act
            var response = await client.PostAsJsonAsync("api/values/1", "boo");

            //Assert
            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
            dynamic content = await response.Content.ReadAsAsync<dynamic>();
            Assert.Equal("Internal Server Error.", content.value.ToObject<string[]>()[0]);
            Assert.Equal("Api.Controllers.MyImportantException", (string)content.error.ClassName);

            dynamic contentString = await response.Content.ReadAsStringAsync();
            _output.WriteLine(contentString);
        }
    }
}
