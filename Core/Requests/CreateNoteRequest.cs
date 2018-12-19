using MediatR;

namespace Core
{
    public class CreateNoteRequest : Request, IRequest<Result<Note>>
    {
        public int InvoiceId { get; set; }
        public string Text { get; set; }
    }
}
