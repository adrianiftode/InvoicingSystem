using System.Collections.Generic;
using MediatR;

namespace Core
{
    public class InvoiceNotesQuery : Request, IRequest<IReadOnlyCollection<Note>>
    {
        public int InvoiceId { get; set; }
    }
}
