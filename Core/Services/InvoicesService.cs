using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Repositories;

namespace Core.Services
{
    public class InvoicesService : IInvoicesService
    {
        private readonly IInvoicesRepository _invoicesRepository;

        public InvoicesService(IInvoicesRepository invoicesRepository)
        {
            _invoicesRepository = invoicesRepository;
        }
        public async Task<Invoice> Get(int id)
        {
            var invoice = await _invoicesRepository.Get(id);
            return invoice;
        }

        public async Task<IReadOnlyCollection<Note>> GetNotes(int id)
        {
            var notes = await _invoicesRepository.GetNotes(id);
            return notes;
        }
    }
}