using Core.Repositories;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Core
{
    public class InvoiceNotesQuery : Request, IRequest<IReadOnlyCollection<Note>>
    {
        public int InvoiceId { get; set; }
    }

    public class InvoiceNotesHandler : IRequestHandler<InvoiceNotesQuery, IReadOnlyCollection<Note>>
    {
        private readonly IInvoicesRepository _invoicesRepository;

        public InvoiceNotesHandler(IInvoicesRepository invoicesRepository)
            => _invoicesRepository = invoicesRepository;

        public async Task<IReadOnlyCollection<Note>> Handle(InvoiceNotesQuery request, CancellationToken cancellationToken = default)
        {
            var notes = await _invoicesRepository.GetNotesBy(request.InvoiceId);
            return notes;
        }
    }
}
