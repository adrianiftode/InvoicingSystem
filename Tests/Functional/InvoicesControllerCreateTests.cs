using Api.Models;
using Core;
using Core.Repositories;
using FluentAssertions;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Net.Http;
using System.Threading.Tasks;
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
            response.Should().Be201Created()
                .And.BeAs(new InvoiceModel
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
            response.Should().Be400BadRequest()
                .And.HaveError("$.amount", "*converted*Decimal*$.amount*");
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
            response.Should().Be400BadRequest()
                .And.HaveError("Identifier", "*Identifier*required*");
        }

        [Fact]
        public async Task Create_WithoutBody_ShouldReturnBadRequest()
        {
            //Act
            var response = await _client.PostAsJsonAsync("/invoices", default(object));

            //Assert
            response.Should().Be400BadRequest()
                .And.HaveErrorMessage("A non-empty request body is required.");
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
            response.Should().Be201Created()
                .And.BeAs(new InvoiceModel
                {
                    Identifier = "INV-001",
                    Amount = 150.05m
                });
        }
    }
}
