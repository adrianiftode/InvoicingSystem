using MediatR;

namespace Core
{
    public class UpdateInvoiceRequest : Request, IRequest<Result<Invoice>>
    {
        public int InvoiceId { get; set; }
        public string Identifier { get; set; }
        public decimal Amount { get; set; }
    }
}
