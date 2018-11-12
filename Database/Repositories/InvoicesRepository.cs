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
                .Include(c=>c.UpdatedBy)
                .Include(c => c.Notes)
                .FirstOrDefaultAsync(c => c.InvoiceId == id);

            if (entity == null)
            {
                return null;
            }

            return new Core.Invoice
            {
                Identifier = entity.Identifier,
                Amount = entity.Amount,
                InvoiceId = entity.InvoiceId,
                Notes = entity.Notes.Select(c => c.Text).ToList(),
                UpdatedBy = entity.UpdatedBy.Identity
            };
        }

        public async Task<IReadOnlyCollection<Core.Note>> GetNotes(int id)
        {
            var entity = await _context.Invoices
                .Include(c => c.UpdatedBy)
                .Include(c => c.Notes)
                    .ThenInclude( n => n.UpdatedBy)
                .FirstOrDefaultAsync(c => c.InvoiceId == id);

            return entity?.Notes.Select(c => new Core.Note
            {
                Text = c.Text,
                UpdatedBy = c.UpdatedBy.Identity
            }).ToList();
        }
    }
}
