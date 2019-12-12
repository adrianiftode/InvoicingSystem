using Core.Pipeline;
using Core.Repositories;
using FluentValidation;
using FluentValidation.Validators;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Core
{
    public class UpdateInvoiceRequest : Request, IRequest<(Invoice invoice, Result result)>
    {
        public int InvoiceId { get; set; }
        public string Identifier { get; set; }
        public decimal Amount { get; set; }
    }

    public class UpdateInvoiceValidator : AbstractValidator<UpdateInvoiceRequest>
    {
        private readonly IInvoicesRepository _repository;

        public UpdateInvoiceValidator(IInvoicesRepository repository)
        {
            _repository = repository;

            RuleFor(x => x).MustAsync(Exist).WithErrorCode(Result.NotPresent.StatusCode);
            RuleFor(x => x.Identifier)
                .NotEmpty()
                .MustAsync(NotBeUsedByADifferentInvoice)
                    .WithMessage("The invoice cannot be updated " +
                                 "because another invoice with the new identifier already exists.")
                ;
        }

        private async Task<bool> Exist(UpdateInvoiceRequest request, CancellationToken cancellationToken)
        {
            var invoice = await _repository.Get(request.InvoiceId);
            return invoice != null;
        }

        private async Task<bool> NotBeUsedByADifferentInvoice(UpdateInvoiceRequest request,
            string property,
            PropertyValidatorContext propertyValidatorContext,
            CancellationToken cancellationToken)
        {
            var anyWithNewIdentifier = await _repository.GetByIdentifier(request.Identifier);
            return anyWithNewIdentifier == null || anyWithNewIdentifier.InvoiceId == request.InvoiceId;
        }
    }

    public class UpdateInvoiceAuthorization : IAuthorize<UpdateInvoiceRequest>
    {
        private readonly IInvoicesRepository _repository;

        public UpdateInvoiceAuthorization(IInvoicesRepository invoicesRepository)
        {
            _repository = invoicesRepository;
        }

        public async Task<bool> Authorize(UpdateInvoiceRequest request)
        {
            var note = await _repository.Get(request.InvoiceId);

            return note.UpdatedBy == request.User.GetIdentity();
        }
    }

    public class UpdateInvoiceHandler : IRequestHandler<UpdateInvoiceRequest, (Invoice invoice, Result result)>
    {
        private readonly IInvoicesRepository _invoicesRepository;

        public UpdateInvoiceHandler(IInvoicesRepository invoicesRepository)
        {
            _invoicesRepository = invoicesRepository;
        }
        public async Task<(Invoice invoice, Result result)> Handle(UpdateInvoiceRequest request, CancellationToken cancellationToken = default)
        {
            var invoice = await _invoicesRepository.Get(request.InvoiceId);

            invoice.UpdatedBy = request.User.GetIdentity();
            invoice.Identifier = request.Identifier;
            invoice.Amount = request.Amount;

            await _invoicesRepository.Update();
            return invoice;
        }
    }
}
