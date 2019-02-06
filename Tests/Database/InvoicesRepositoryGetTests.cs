using Database;
using Database.Repositories;
using FluentAssertions;
using System;
using System.Threading.Tasks;
using Tests.Extensions.Database;
using Xunit;

namespace Tests.Database
{
    public class InvoicesRepositoryGetTests
    {
        [Theory]
        [Contexts]
        public async Task GetById_WhenExists_ReturnsInvoice(Func<string, InvoicingContext> factory)
        {
            //Arrange
            using (var context = factory(nameof(GetById_WhenExists_ReturnsInvoice)))
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

        [Theory]
        [Contexts]
        public async Task GetNotesByInvoice_WhenNotPresent_ReturnsNull(Func<string, InvoicingContext> factory)
        {
            //Arrange
            using (var context = factory(nameof(GetNotesByInvoice_WhenNotPresent_ReturnsNull)))
            {
                var sut = new InvoicesRepository(context);

                //Act
                var notes = await sut.GetNotesBy(100);

                //Assert
                notes.Should().BeNull();
            }
        }

        [Theory]
        [Contexts]
        public async Task GetNotesByInvoice_WhenInvoiceExistsButHasNoNotes_ReturnsEmpty(Func<string, InvoicingContext> factory)
        {
            //Arrange
            using (var context = factory(nameof(GetNotesByInvoice_WhenInvoiceExistsButHasNoNotes_ReturnsEmpty)))
            {
                var sut = new InvoicesRepository(context);

                //Act
                var notes = await sut.GetNotesBy(2);

                //Assert
                notes.Should().NotBeNull();
                notes.Should().BeEmpty();
            }
        }

        [Theory]
        [Contexts]
        public async Task GetNotesByInvoiceId_WhenExists_ReturnsNotes(Func<string, InvoicingContext> factory)
        {
            //Arrange
            using (var context = factory(nameof(GetNotesByInvoiceId_WhenExists_ReturnsNotes)))
            {
                var invoiceId = 1;
                var sut = new InvoicesRepository(context);

                //Act
                var notes = await sut.GetNotesBy(invoiceId);

                //Assert
                notes.Should().NotBeNullOrEmpty();
            }
        }

        [Theory]
        [Contexts]
        public async Task GetById_WhenNotPresent_ReturnsNull(Func<string, InvoicingContext> factory)
        {
            //Arrange
            using (var context = factory(nameof(GetById_WhenNotPresent_ReturnsNull)))
            {
                var sut = new InvoicesRepository(context);

                //Act
                var invoice = await sut.Get(100);

                //Assert
                invoice.Should().BeNull();
            }
        }
    }
}
