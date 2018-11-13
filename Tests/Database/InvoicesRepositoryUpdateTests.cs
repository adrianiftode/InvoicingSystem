using Database;
using Database.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Core;
using Xunit;

namespace Tests.Database
{
    public class InvoicesRepositoryUpdateTests
    {
        [Fact]
        public async Task UpdateInvoice_WhenInvoiceValid_ReturnsInvoice()
        {
            //Arrange
            int nextInvoiceId = 1;
            using (var context = CreateContext(nameof(UpdateInvoice_WhenInvoiceValid_ReturnsInvoice)))
            {
                var invoice = new Invoice
                {
                    InvoiceId = nextInvoiceId,
                    Amount =  1,
                    Identifier = "INV-01"
                };

                context.Invoices.Add(invoice);
                await context.SaveChangesAsync();
            }

            //Act
            using (var context = CreateContext(nameof(UpdateInvoice_WhenInvoiceValid_ReturnsInvoice)))
            {
                var sut = new InvoicesRepository(context);

                var invoice = context.Invoices.Find(nextInvoiceId);
                invoice.Amount = 100;

                await sut.Update();
            }

            //Assert
            using (var context = CreateContext(nameof(UpdateInvoice_WhenInvoiceValid_ReturnsInvoice)))
            {
                var invoice = context.Invoices.Find(nextInvoiceId);

                invoice.Amount.Should().Be(100);
            }
        }

        private static InvoicingContext CreateContext(string databaseName)
        {
            var options = new DbContextOptionsBuilder<InvoicingContext>()
                .UseInMemoryDatabase(databaseName)
                .Options;
            var context = new InvoicingContext(options); // for this test we don't rely on HasData, so EnsureCreated is not used
            return context;
        }
    }
}
