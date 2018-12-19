using MediatR;

namespace Core
{
    public class InvoiceByIdQuery : Request, IRequest<Invoice>
    {
        public int Id { get; set; }
    }
}
