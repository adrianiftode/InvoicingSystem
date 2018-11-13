using System.Collections.Generic;
using System.Linq;

namespace Core
{
    public class Invoice
    {
        public int InvoiceId { get; internal set; }
        public string Identifier { get; internal set; }
        public decimal Amount { get; internal set; }
        public ICollection<Note> Notes { get; internal set; }
        public string UpdatedBy { get; internal set; }

        public Note AddNote(string note, string updatedBy)
        {
            if (Notes.Any(n => n.Text == note))
            {
                return null;
            }

            var newNote = new Note
            {
                Text = note,
                UpdatedBy = updatedBy
            };
            Notes.Add(newNote);
            return newNote;
        }
    }
}