using System.Collections.Generic;
using Core.Repositories;
using System.Linq;
using System.Threading.Tasks;
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

        public async Task<Core.Invoice> Get(int id)
        {
            var entity = await _context.Invoices
                .Include(c => c.Notes)
                .FirstOrDefaultAsync(c => c.InvoiceId == id);

            return entity;
        }

        public async Task<IReadOnlyCollection<Core.Note>> GetNotesBy(int invoiceId)
        {
            var entity = await _context.Invoices
                .Include(c => c.Notes)
                .FirstOrDefaultAsync(c => c.InvoiceId == invoiceId);

            return entity?.Notes.ToList();
        }
    }
}
