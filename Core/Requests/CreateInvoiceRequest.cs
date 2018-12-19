using MediatR;

namespace Core
{
    public class CreateInvoiceRequest : Request, IRequest<Result<Invoice>>
    {
        public string Identifier { get; set; }
        public decimal Amount { get; set; }
    }
}
