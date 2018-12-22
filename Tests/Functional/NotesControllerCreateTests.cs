using Core;
using Core.Repositories;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Net.Http;
using System.Threading.Tasks;
using Api.Models;
using Tests.Functional.Extensions;
using Tests.Functional.Fixtures;
using Tests.Extensions.FluentAssertions;
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
            (await response.Should().BeCreated<NoteModel>())
                .And.BeEquivalentTo(new
                {
                    Text = "Text",
                    NoteId = 2
                });
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
            (await response.Should().BeBadRequest())
                .WithError("invoiceId", "Could not convert string to int");
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
            await response.Should().BeNotFound();
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
            (await response.Should().BeBadRequest())
                .WithError("Text", "The Text field is required.");

        }

        [Fact]
        public async Task Create_WithoutBody_ShouldReturnBadRequest()
        {
            //Act
            var response = await _defaultClient.PostAsJsonAsync("/notes", default(object));

            //Assert
            (await response.Should().BeBadRequest())
                .WithError("", "A non-empty request body is required.");
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
            (await response.Should().BeCreated<NoteModel>())
                .And.BeEquivalentTo(new
                {
                    Text = "text"
                });
        }
    }
}
