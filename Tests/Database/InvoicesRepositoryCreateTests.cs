using Core;
using Database;
using Database.Repositories;
using FluentAssertions;
using System;
using System.Threading.Tasks;
using Tests.Extensions.Database;
using Xunit;

namespace Tests.Database
{
    public class InvoicesRepositoryCreateTests
    {
        [Theory]
        [Contexts]
        public async Task CreateInvoice_WhenInvoiceValid_ReturnsInvoice(Func<string, InvoicingContext> factory)
        {
            //Arrange
            using (var context = factory(nameof(CreateInvoice_WhenInvoiceValid_ReturnsInvoice)))
            {
                var nextInvoiceId = await context.GetNextInvoiceId();
                var invoice = new Invoice
                {
                    InvoiceId = nextInvoiceId,
                    Identifier = "INV-" + nextInvoiceId,
                    UpdatedBy = "Test"
                };
                var sut = new InvoicesRepository(context);

                //Act
                await sut.Create(invoice);

                //Assert
                invoice.Should().NotBeNull();
                invoice.InvoiceId.Should().BeGreaterThan(0);
            }
        }
    }
}
