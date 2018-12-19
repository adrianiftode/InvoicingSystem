using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Core.Repositories;
using MediatR;

namespace Core.Handlers
{
    public class InvoiceNotesHandler : IRequestHandler<InvoiceNotesQuery, IReadOnlyCollection<Note>>
    {
        private readonly IInvoicesRepository _invoicesRepository;

        public InvoiceNotesHandler(IInvoicesRepository invoicesRepository)
        {
            _invoicesRepository = invoicesRepository;
        }

        public async Task<IReadOnlyCollection<Note>> Handle(InvoiceNotesQuery request, CancellationToken cancellationToken = default(CancellationToken))
        {
            var notes = await _invoicesRepository.GetNotesBy(request.InvoiceId);
            return notes;
        }
    }
}