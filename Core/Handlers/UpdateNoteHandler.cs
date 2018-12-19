using System.Threading;
using System.Threading.Tasks;
using Core.Repositories;
using MediatR;

namespace Core.Handlers
{
    public class UpdateNoteHandler : IRequestHandler<UpdateNoteRequest, Result<Note>>
    {
        private readonly INotesRepository _notesRepository;

        public UpdateNoteHandler(INotesRepository notesRepository)
        {
            _notesRepository = notesRepository;
        }

        public async Task<Result<Note>> Handle(UpdateNoteRequest request, CancellationToken cancellationToken = default(CancellationToken))
        {
            var note = await _notesRepository.Get(request.NoteId);

            if (note == null)
            {
                return Result.NotPresent;
            }

            if (note.UpdatedBy != request.User.GetIdentity())
            {
                return Result.Forbidden;
            }

            note.UpdatedBy = request.User.GetIdentity();
            note.Text = request.Text;

            await _notesRepository.Update();
            return note;
        }
    }
}