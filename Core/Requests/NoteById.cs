using System.Threading;
using System.Threading.Tasks;
using Core.Repositories;
using MediatR;

namespace Core
{
    public class NoteByIdQuery : Request, IRequest<Note>
    {
        public int Id { get; set; }
    }

    public class NoteByIdHandler : IRequestHandler<NoteByIdQuery, Note>
    {
        private readonly INotesRepository _notesRepository;

        public NoteByIdHandler(INotesRepository notesRepository)
        {
            _notesRepository = notesRepository;
        }

        public async Task<Note> Handle(NoteByIdQuery request, CancellationToken cancellationToken = default(CancellationToken))
        {
            var note = await _notesRepository.Get(request.Id);
            return note;
        }
    }
}
