using Api.Models;
using System.Net.Http;
using System.Threading.Tasks;
using Tests.Extensions.FluentAssertions;
using Tests.Functional.Extensions;
using Tests.Functional.Fixtures;
using Xunit;

namespace Tests.Functional
{

    public class InvoicesControllerUpdateTests : IClassFixture<InMemoryWebApplicationFactory>
    {
        private readonly InMemoryWebApplicationFactory _factory;
        private readonly HttpClient _client;

        public InvoicesControllerUpdateTests(InMemoryWebApplicationFactory factory)
        {
            _factory = factory;
            _client = _factory               
               .CreateClient("user123");
        }

        [Fact]
        public async Task Update_WithValidRequest_ShouldReturnUpdatedWithInvoice()
        {
            //Act
            var response = await _client.PutAsJsonAsync("/invoices", new
            {
                invoiceId = 1,
                identifier = "INV-001",
                amount = 50.05m
            });

            //Assert
            (await response.Should().BeOk<InvoiceModel>())
                .And.BeEquivalentTo(new 
                {
                    InvoiceId = 1,
                    Amount = 50.05m,
                    Identifier = "INV-001"
                });
        }

        [Fact]
        public async Task Update_WithNotValidAmount_ShouldReturnBadRequest()
        {
            //Act
            var response = await _client.PutAsJsonAsync("/invoices", new
            {
                amount = "Invalid Decimal Value"
            });

            //Assert
            (await response.Should().BeBadRequest())
                .WithError("amount", "Could not convert string to decimal");
        }

        [Fact]
        public async Task Update_WithoutIdentifier_ShouldReturnBadRequest()
        {
            //Act
            var response = await _client.PutAsJsonAsync("/invoices", new
            {
                identifier = (string)null
            });

            //Assert
            (await response.Should().BeBadRequest())
                .WithError("Identifier", "The Identifier field is required.");
        }

        [Fact]
        public async Task Update_WithoutBody_ShouldReturnBadRequest()
        {
            //Act
            var response = await _client.PutAsJsonAsync("/invoices", default(object));

            //Assert
            (await response.Should().BeBadRequest())
                .WithError("", "A non-empty request body is required.");
        }
    }
}
