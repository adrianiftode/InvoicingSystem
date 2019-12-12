using System;
using System.Collections.Generic;
using System.Linq;
using Core;

namespace Api.Models
{
    public class InvoiceModel
    {
        public int InvoiceId { get; set; }
        public decimal Amount { get; set; }
        public IReadOnlyCollection<NoteModel> Notes { get; set; } = Array.Empty<NoteModel>();
        public string Identifier { get; set; }
    }

    public static class InvoiceMapper
    {
        public static InvoiceModel Map(this Invoice invoice)
            => invoice != null ? new InvoiceModel
            {
                Amount = invoice.Amount,
                InvoiceId = invoice.InvoiceId,
                Identifier = invoice.Identifier,
                Notes = invoice.Notes.Select(n => n.Map()).ToList()
            } : null;

        public static InvoiceModel Map(this (Invoice, Result) result)
            => result.Item1.Map();

        public static IEnumerable<InvoiceModel> Map(this IEnumerable<Invoice> invoices)
            => invoices.Select(Map);
    }
}
