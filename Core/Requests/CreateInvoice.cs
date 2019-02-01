using System.Threading;
using System.Threading.Tasks;
using Core.Pipeline;
using Core.Repositories;
using FluentValidation;
using FluentValidation.Validators;
using MediatR;

namespace Core
{
    public class CreateInvoiceRequest : Request, IRequest<Result<Invoice>>
    {
        public string Identifier { get; set; }
        public decimal Amount { get; set; }
    }

    public class CreateInvoiceAuthorization : IAuthorize<CreateInvoiceRequest>
    {
        public Task<bool> Authorize(CreateInvoiceRequest request)
        {
            return Task.FromResult(request.User != null && request.User.IsAdmin());
        }
    }

    public class CreateInvoiceValidator : AbstractValidator<CreateInvoiceRequest>
    {
        private readonly IInvoicesRepository _repository;

        public CreateInvoiceValidator(IInvoicesRepository invoicesRepository)
        {
            _repository = invoicesRepository;

            RuleFor(x => x.Identifier)
                .MustAsync(NotBeUsedByADifferentInvoice)
                .WithMessage("The invoice cannot be created because another invoice with the same Identifier already exists.");
        }

        private async Task<bool> NotBeUsedByADifferentInvoice(CreateInvoiceRequest request, 
            string property, 
            PropertyValidatorContext propertyValidatorContext, 
            CancellationToken cancellationToken)
        {
            var anyWithNewIdentifier = await _repository.GetByIdentifier(request.Identifier);
            return anyWithNewIdentifier == null;
        }

        public async Task<bool> NotAlreadyExists(CreateInvoiceRequest request, CancellationToken cancellationToken)
        {
            return await _repository.GetByIdentifier(request.Identifier) == null;
        }
    }

    public class CreateInvoiceHandler : IRequestHandler<CreateInvoiceRequest, Result<Invoice>>
    {
        private readonly IInvoicesRepository _invoicesRepository;

        public CreateInvoiceHandler(IInvoicesRepository invoicesRepository)
        {
            _invoicesRepository = invoicesRepository;
        }
        public async Task<Result<Invoice>> Handle(CreateInvoiceRequest request, CancellationToken cancellationToken = default(CancellationToken))
        {
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
