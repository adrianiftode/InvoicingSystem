using Core.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Handlers
{
    public class CreateInvoiceHandler : IRequestHandler<CreateInvoiceRequest, Result<Invoice>>
    {
        private readonly IInvoicesRepository _invoicesRepository;

        public CreateInvoiceHandler(IInvoicesRepository invoicesRepository)
        {
            _invoicesRepository = invoicesRepository;
        }
        public async Task<Result<Invoice>> Handle(CreateInvoiceRequest request, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (!request.User.IsAdmin())
            {
                return Result.Forbidden;
            }

            if (await _invoicesRepository.GetByIdentifier(request.Identifier) != null)
            {
                return Result.Error(
                    "The invoice cannot be created because another invoice with the same Identifier already exists.");
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
