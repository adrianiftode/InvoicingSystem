using MediatR;

namespace Core
{
    public class UpdateNoteRequest : Request, IRequest<Result<Note>>
    {
        public int NoteId { get; set; }
        public string Text { get; set; }
    }
}
