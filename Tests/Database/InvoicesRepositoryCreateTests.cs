using Database;
using Database.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Core;
using Xunit;

namespace Tests.Database
{
    public class InvoicesRepositoryCreateTests
    {
        [Fact]
        public async Task CreateInvoice_WhenInvoiceValid_ReturnsInvoice()
        {
            //Arrange
            using (var context = CreateContext(nameof(CreateInvoice_WhenInvoiceValid_ReturnsInvoice)))
            {
                var nextInvoiceId = await context.Invoices.MaxAsync(c => c.InvoiceId);
                var invoice = new Invoice
                {
                    InvoiceId = nextInvoiceId + 1 //see this why it needs the setup like this https://github.com/aspnet/EntityFrameworkCore/issues/12371
                };
                var sut = new InvoicesRepository(context);

                //Act
                await sut.Create(invoice);

                //Assert
                invoice.Should().NotBeNull();
                invoice.InvoiceId.Should().BeGreaterThan(0);
            }
        }

        private static InvoicingContext CreateContext(string databaseName)
        {
            var options = new DbContextOptionsBuilder<InvoicingContext>()
                .UseInMemoryDatabase(databaseName)
                .Options;
            var context = new InvoicingContext(options);
            context.Database.EnsureCreated(); // this will also call HasData
            return context;
        }
    }
}
