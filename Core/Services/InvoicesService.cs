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

        public async Task<IReadOnlyCollection<Note>> GetNotesBy(int invoiceId)
        {
            var notes = await _invoicesRepository.GetNotesBy(invoiceId);
            return notes;
        }

        public async Task<AddNoteResponse> AddNote(AddNoteRequest request)
        {
            var invoice = await _invoicesRepository.Get(request.InvoiceId);

            var note = invoice.AddNote(request.Text, request.User.GetIdentity());

            //_invoicesRepository.Update(invoice);
            //var note = await _invoicesRepository.GetNotes()
            return new AddNoteResponse
            {
                //Item = invoice.Notes.LastOrDefault();
                Item = note
            };
        }

        public async Task<Invoice> Create(CreateInvoiceRequest request)
        {
            if (await _invoicesRepository.GetByIdentifier(request.Identifier) != null)
            {
                return null;
            }

            if (request.User.IsUser())
            {
                return null;
            }

            var invoice = new Invoice
            {
                Amount = request.Amount,
                Identifier = request.Identifier,
                UpdatedBy = request.User.GetIdentity()
            };

            await _invoicesRepository.Create(invoice);

            return invoice;
        }
    }
}