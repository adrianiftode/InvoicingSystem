//using System.Net;
//using System.Threading.Tasks;
//using Xunit.Abstractions;

//namespace Tests.Functional
//{
//    public class InvoicesControllerTests : IClassFixture<InMemoryWebApplicationFactory>
//    {
//        private readonly InMemoryWebApplicationFactory _factory;
//        private readonly ITestOutputHelper _output;

//        public InvoicesControllerTests(InMemoryWebApplicationFactory factory, ITestOutputHelper output)
//        {
//            _factory = factory;
//            _output = output;
//        }

//        [Theory]
//        [InlineData("invoices", HttpStatusCode.NotFound)]
//        [InlineData("invoices/1", HttpStatusCode.OK)]
//        [InlineData("invoices/2", HttpStatusCode.NotFound)]
//        [InlineData("invoices/string-like-value", HttpStatusCode.BadRequest)]
//        public async Task Get_ShouldReturnExpectedResponse(string path, HttpStatusCode expectedStatusCode)
//        {
//            //Arrange
//            var client = _factory.CreateClient();

//            //Act
//            var response = await client.GetAsync(path);

//            //Assert
//            var content = await response.Content.ReadAsStringAsync();
//            _output.WriteLine(content);
//            response.StatusCode.Should().Be(expectedStatusCode);
//        }
//    }
//}
