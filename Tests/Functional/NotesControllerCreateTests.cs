using Core;
using Core.Repositories;
using FluentAssertions;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Tests.Functional.Extensions;
using Tests.Functional.Fixtures;
using Xunit;

namespace Tests.Functional
{
    public class NotesControllerCreateTests : IClassFixture<InMemoryWebApplicationFactory>
    {
        private readonly InMemoryWebApplicationFactory _factory;
        private readonly HttpClient _defaultClient;

        public NotesControllerCreateTests(InMemoryWebApplicationFactory factory)
        {
            _factory = factory;
            _defaultClient = _factory.CreateClient("user123");
        }

        [Fact]
        public async Task Create_WithValidRequest_ShouldReturnCreatedWithInvoice()
        {
            // Arrange
            var client = _factory
                .WithResponse<CreateNoteRequest, Result<Note>>(new Note
                {
                    NoteId = 2,
                    Text = "Text"
                })
                .CreateClient("user123");

            // Act
            var response = await client.PostAsJsonAsync("/notes", new
            {
                invoiceId = 1,
                text = "Text"
            });

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created, await response.Content.ReadAsStringAsync());
            var content = await response.Content.ReadAsAsync<dynamic>();
            ((string)content.text).Should().Be("Text");
            ((int)content.noteId).Should().Be(2);
        }

        [Fact]
        public async Task Create_WithNotValidValueForInvoiceId_ShouldReturnBadRequest()
        {
            //Act
            var response = await _defaultClient.PostAsJsonAsync("/notes", new
            {
                invoiceId = "not valid"
            });

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            dynamic content = await response.Content.ReadAsAsync<dynamic>();
            ((string[])content.invoiceId.ToObject<string[]>())
                [0].Should().Contain("Could not convert string to int");
        }

        [Fact]
        public async Task Create_WithNotFoundResult_ShouldReturnNotFound()
        {
            // Arrange
            var client = _factory
                    .WithResponse<CreateNoteRequest, Result<Note>>(Result.NotPresent)
                    .CreateClient("admin123");

            //Act
            var response = await client.PostAsJsonAsync("/notes", new
            {
                invoiceId = 1,
                text = "Note"
            });

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound, await response.Content.ReadAsStringAsync());
        }

        [Fact]
        public async Task Create_WithoutText_ShouldReturnBadRequest()
        {
            //Act
            var response = await _defaultClient.PostAsJsonAsync("/notes", new
            {
                text = (string)null,
                invoiceId = 1
            });

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            dynamic content = await response.Content.ReadAsAsync<dynamic>();
            ((string[])content.Text.ToObject<string[]>())
                [0].Should().Contain("The Text field is required.");

        }

        [Fact]
        public async Task Create_WithoutBody_ShouldReturnBadRequest()
        {
            //Act
            var response = await _defaultClient.PostAsJsonAsync("/notes", default(object));

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            dynamic content = await response.Content.ReadAsAsync<dynamic>();
            ((string[])content[""].ToObject<string[]>())
                [0].Should().Contain("A non-empty request body is required.");
        }

        [Fact]
        public async Task Create_WithAuthenticatedUser_ShouldReturnCreatedWithInvoice()
        {
            //Arrange
            var client = _factory
                .WithWebHostBuilder(c =>
                {
                    var repository = new Mock<INotesRepository>();
                    c.ConfigureTestServices(srv => srv.AddTransient(_ => repository.Object));
                })
                .CreateClient("admin123");

            //Act
            var response = await client.PostAsJsonAsync("/notes", new
            {
                invoiceId = 1,
                text = "text"
            });

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created, await response.Content.ReadAsStringAsync());
        }
    }
}
