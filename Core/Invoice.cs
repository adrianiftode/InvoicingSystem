using System.Collections.Generic;

namespace Core
{
    public class Invoice
    {
        public int InvoiceId { get; set; }
        public string Identifier { get; set; }
        public decimal Amount { get; set; }
        public IReadOnlyCollection<string> Notes { get; set; }
        public string UpdatedBy { get; set; }
    }
}