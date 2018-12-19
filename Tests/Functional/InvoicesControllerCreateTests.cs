using Core;
using Core.Repositories;
using FluentAssertions;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Tests.Fixtures;
using Xunit;

namespace Tests.Functional
{
    public class InvoicesControllerCreateTests : IClassFixture<InMemoryWebApplicationFactory>
    {
        private readonly InMemoryWebApplicationFactory _factory;
        private readonly HttpClient _client;

        public InvoicesControllerCreateTests(InMemoryWebApplicationFactory factory)
        {
            _factory = factory;
            _client = _factory
                .WithWebHostBuilder(c =>
                {
                    var invoicesServiceMock = new Mock<IMediator>();
                    invoicesServiceMock
                        .Setup(m => m.Send(It.IsAny<CreateInvoiceRequest>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(new Invoice
                        {
                            Identifier = "INV-001",
                            Amount = 150.05m
                        });
                    c.ConfigureTestServices(srv =>
                    {
                        srv.AddTransient(_ => invoicesServiceMock.Object);
                    });
                })
                .CreateClient("user123");
        }

        [Fact]
        public async Task Create_WithValidRequest_ShouldReturnCreatedWithInvoice()
        {
            //Act
            var response = await _client.PostAsJsonAsync("/invoices", new
            {
                identifier = "INV-001",
                amount = 150.05m
            });

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            var content = await response.Content.ReadAsAsync<dynamic>();
            ((decimal)content.amount).Should().Be(150.05m);
        }



        [Fact]
        public async Task Create_WithNotValidAmount_ShouldReturnBadRequest()
        {
            //Act
            var response = await _client.PostAsJsonAsync("/invoices", new
            {
                amount = "asd"
            });

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            dynamic content = await response.Content.ReadAsAsync<dynamic>();
            ((string[])content.amount.ToObject<string[]>())
                [0].Should().Contain("Could not convert string to decimal");
        }

        [Fact]
        public async Task Create_WithoutIdentifier_ShouldReturnBadRequest()
        {
            //Act
            var response = await _client.PostAsJsonAsync("/invoices", new
            {
                identifier = (string)null
            });

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            dynamic content = await response.Content.ReadAsAsync<dynamic>();
            ((string[])content.Identifier.ToObject<string[]>())
                [0].Should().Contain("The Identifier field is required."); //for the PascalCase style in the error messages see this discussion https://github.com/aspnet/Mvc/issues/5590

        }

        [Fact]
        public async Task Create_WithoutBody_ShouldReturnBadRequest()
        {
            //Act
            var response = await _client.PostAsJsonAsync("/invoices", default(object));

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
                    var invoicesRepositoryMock = new Mock<IInvoicesRepository>();
                    c.ConfigureTestServices(srv => srv.AddTransient(_ => invoicesRepositoryMock.Object));
                })
                .CreateClient("admin123");

            //Act
            var response = await client.PostAsJsonAsync("/invoices", new
            {
                identifier = "INV-001",
                amount = 150.05m
            });

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created, await response.Content.ReadAsStringAsync());
        }
    }
}
