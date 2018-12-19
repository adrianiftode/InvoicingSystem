using MediatR;

namespace Core
{
    public class NoteByIdQuery : Request, IRequest<Note>
    {
        public int Id { get; set; }
    }
}
