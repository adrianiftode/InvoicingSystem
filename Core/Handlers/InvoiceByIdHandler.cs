using System.Threading;
using System.Threading.Tasks;
using Core.Repositories;
using MediatR;

namespace Core.Handlers
{
    public class InvoiceByIdHandler : IRequestHandler<InvoiceByIdQuery, Invoice>
    {
        private readonly IInvoicesRepository _invoicesRepository;

        public InvoiceByIdHandler(IInvoicesRepository invoicesRepository)
        {
            _invoicesRepository = invoicesRepository;
        }

        public async Task<Invoice> Handle(InvoiceByIdQuery request, CancellationToken cancellationToken = default(CancellationToken))
        {
            var invoice = await _invoicesRepository.Get(request.Id);
            return invoice;
        }
    }
}