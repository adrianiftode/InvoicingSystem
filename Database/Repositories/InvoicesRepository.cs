using System.Collections.Generic;
using Core.Repositories;
using System.Linq;
using System.Threading.Tasks;
using Core;
using Microsoft.EntityFrameworkCore;

namespace Database.Repositories
{
    public class InvoicesRepository : IInvoicesRepository
    {
        private readonly InvoicingContext _context;

        public InvoicesRepository(InvoicingContext context)
        {
            _context = context;
        }

        public async Task<Invoice> Get(int id)
        {
            var entity = await _context.Invoices
                .Include(c => c.Notes)
                .FirstOrDefaultAsync(c => c.InvoiceId == id);

            return entity;
        }

        public async Task<Invoice> GetByIdentifier(string identifier)
        {
            var entity = await _context.Invoices
                .Include(c => c.Notes)
                .FirstOrDefaultAsync(c => c.Identifier == identifier);

            return entity;
        }

        public async Task<IReadOnlyCollection<Note>> GetNotesBy(int invoiceId)
        {
            var entity = await _context.Invoices
                .Include(c => c.Notes)
                .FirstOrDefaultAsync(c => c.InvoiceId == invoiceId);

            return entity?.Notes.ToList();
        }

        public async Task Create(Invoice invoice)
        {
            _context.Invoices.Add(invoice);
            await _context.SaveChangesAsync();
        }
    }

}
