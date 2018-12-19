using Core.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Handlers
{
    public class UpdateInvoiceHandler : IRequestHandler<UpdateInvoiceRequest, Result<Invoice>>
    {
        private readonly IInvoicesRepository _invoicesRepository;

        public UpdateInvoiceHandler(IInvoicesRepository invoicesRepository)
        {
            _invoicesRepository = invoicesRepository;
        }
        public async Task<Result<Invoice>> Handle(UpdateInvoiceRequest request, CancellationToken cancellationToken = default(CancellationToken))
        {
            var invoice = await _invoicesRepository.Get(request.InvoiceId);

            if (invoice == null)
            {
                return Result.NotPresent;
            }

            if (invoice.UpdatedBy != request.User.GetIdentity())
            {
                return Result.Forbidden;
            }

            var withNewIdentifier = await _invoicesRepository.GetByIdentifier(request.Identifier);

            if (withNewIdentifier != null && withNewIdentifier.InvoiceId != invoice.InvoiceId)
            {
                return Result.Error("The invoice cannot be updated because another invoice with the new identifier already exists.");
            }

            invoice.UpdatedBy = request.User.GetIdentity();
            invoice.Identifier = request.Identifier;
            invoice.Amount = request.Amount;

            await _invoicesRepository.Update();
            return invoice;
        }
    }
}
