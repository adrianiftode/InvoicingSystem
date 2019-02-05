using System;
using System.Collections.Generic;

namespace Core
{
    public partial class Invoice
    {
        public int InvoiceId { get; internal set; }
        public string Identifier { get; internal set; }
        public decimal Amount { get; internal set; }
        public ICollection<Note> Notes { get; internal set; } = new List<Note>();
        public string UpdatedBy { get; internal set; }

        public Note AddNote(string note, string updatedBy)
        {
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