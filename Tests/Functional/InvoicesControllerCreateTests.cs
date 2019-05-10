using Api.Models;
using Core;
using Core.Repositories;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Net.Http;
using System.Threading.Tasks;
using Tests.Extensions.FluentAssertions;
using Tests.Functional.Extensions;
using Tests.Functional.Fixtures;
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
                .WithResponse<CreateInvoiceRequest, (Invoice invoice, Result result)>(new Invoice
                {
                    Identifier = "INV-001",
                    Amount = 150.05m
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
            (await response.Should().BeCreated<InvoiceModel>())
                .And.BeEquivalentTo(new InvoiceModel
                {
                    Amount = 150.05m,
                    Identifier = "INV-001"
                });
        }

        [Fact]
        public async Task Create_WithNotValidAmount_ShouldReturnBadRequest()
        {
            //Act
            var response = await _client.PostAsJsonAsync("/invoices", new
            {
                amount = "Invalid Decimal Value"
            });

            //Assert
            (await response.Should().BeBadRequest())
                .WithError("amount", "Could not convert string to decimal");
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
            (await response.Should().BeBadRequest())
                .WithError("Identifier", "The Identifier field is required.");
        }

        [Fact]
        public async Task Create_WithoutBody_ShouldReturnBadRequest()
        {
            //Act
            var response = await _client.PostAsJsonAsync("/invoices", default(object));

            //Assert
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
            (await response.Should().BeCreated<InvoiceModel>())
                .And.BeEquivalentTo(new InvoiceModel
                {
                    Identifier = "INV-001",
                    Amount = 150.05m
                });
        }
    }
}
