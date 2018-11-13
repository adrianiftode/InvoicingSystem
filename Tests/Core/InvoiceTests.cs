using Core;
using FluentAssertions;
using Xunit;

namespace Tests.Core
{
    public class InvoiceTests
    {
        [Fact]
        public void AddNote_IsContainedInNotes()
        {
            //Arrange
            var invoice = new Invoice();

            //Act
            var note = invoice.AddNote("Note", "1");

            //Assert
            note.Text.Should().Be("Note");
            note.UpdatedBy.Should().Be("1");
            invoice.Notes.Should().Contain(note);
        }

        [Fact]
        public void AddNote_WhenCalledWithTheSameText_AddsEachTime()
        {
            //Arrange
            var invoice = new Invoice();

            //Act
            var note1 = invoice.AddNote("Note", "1");
            var note2 = invoice.AddNote("Note", "1");

            //Assert
            invoice.Notes.Should().BeEquivalentTo(note1, note2);
        }
    }
}
