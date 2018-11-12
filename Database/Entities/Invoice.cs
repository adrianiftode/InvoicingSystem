using System.Collections.Generic;

namespace Database.Entities
{
    public class Invoice
    {
        public int InvoiceId { get; set; }
        public string Identifier { get; set; }
        public decimal Amount { get; set; }
        public IReadOnlyCollection<Note> Notes { get; set; } = new List<Note>();
        public int UpdatedByUserId { get; set; }
        public User UpdatedBy { get; set; }
    }
}