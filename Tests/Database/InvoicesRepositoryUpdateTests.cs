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
    public class InvoicesRepositoryUpdateTests
    {
        [Theory]
        [Contexts]
        public async Task UpdateInvoice_WhenInvoiceValid_ReturnsInvoice(Func<string, InvoicingContext> factory)
        {
            //Arrange
            var db = nameof(UpdateInvoice_WhenInvoiceValid_ReturnsInvoice);
            int nextInvoiceId;
            using (var context = factory(db))
            {
                nextInvoiceId = await context.GetNextInvoiceId();
                var invoice = new Invoice
                {
                    InvoiceId = nextInvoiceId,
                    Amount = 1,
                    Identifier = "INV-" + nextInvoiceId,
                    UpdatedBy = "Test"
                };

                context.Invoices.Add(invoice);
                await context.SaveChangesAsync();
            }

            //Act
            using (var context = factory(db))
            {
                var sut = new InvoicesRepository(context);

                var invoice = context.Invoices.Find(nextInvoiceId);
                invoice.Amount = 100;

                await sut.Update();
            }

            //Assert
            using (var context = factory(nameof(UpdateInvoice_WhenInvoiceValid_ReturnsInvoice)))
            {
                var invoice = context.Invoices.Find(nextInvoiceId);

                invoice.Amount.Should().Be(100);
            }
        }
    }
}
