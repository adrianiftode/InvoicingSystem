using Database;
using Database.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Xunit;

namespace Tests.Database
{
    public class InvoicesRepositoryTests
    {
        [Fact]
        public async Task GetById_WhenExists_ReturnsInvoice()
        {
            //Arrange
            using (var context = CreateContext(nameof(GetById_WhenExists_ReturnsInvoice)))
            {
                var invoiceId = 1;
                var sut = new InvoicesRepository(context);

                //Act
                var invoice = await sut.Get(invoiceId);

                //Assert
                invoice.Should().NotBeNull();
                invoice.InvoiceId.Should().Be(invoiceId);
            }
        }

        [Fact]
        public async Task GetNotesByInvoice_WhenNotPresent_ReturnsNull()
        {
            //Arrange
            using (var context = CreateContext(nameof(GetNotesByInvoice_WhenNotPresent_ReturnsNull)))
            {
                var sut = new InvoicesRepository(context);

                //Act
                var notes = await sut.GetNotesBy(100);

                //Assert
                notes.Should().BeNull();
            }
        }

        [Fact]
        public async Task GetNotesByInvoice_WhenInvoiceExistsButHasNoNotes_ReturnsEmpty()
        {
            //Arrange
            using (var context = CreateContext(nameof(GetNotesByInvoice_WhenInvoiceExistsButHasNoNotes_ReturnsEmpty)))
            {
                var sut = new InvoicesRepository(context);

                //Act
                var notes = await sut.GetNotesBy(2);

                //Assert
                notes.Should().NotBeNull();
                notes.Should().BeEmpty();
            }
        }

        [Fact]
        public async Task GetNotesByInvoiceId_WhenExists_ReturnsNotes()
        {
            //Arrange
            using (var context = CreateContext(nameof(GetNotesByInvoiceId_WhenExists_ReturnsNotes)))
            {
                var invoiceId = 1;
                var sut = new InvoicesRepository(context);

                //Act
                var notes = await sut.GetNotesBy(invoiceId);

                //Assert
                notes.Should().NotBeNullOrEmpty();
            }
        }

        [Fact]
        public async Task GetById_WhenNotPresent_ReturnsNull()
        {
            //Arrange
            using (var context = CreateContext(nameof(GetById_WhenNotPresent_ReturnsNull)))
            {
                var sut = new InvoicesRepository(context);

                //Act
                var invoice = await sut.Get(100);

                //Assert
                invoice.Should().BeNull();
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
