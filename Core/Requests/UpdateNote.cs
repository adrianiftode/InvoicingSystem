using Core.Pipeline;
using Core.Repositories;
using FluentValidation;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Core
{
    public class UpdateNoteRequest : Request, IRequest<Result<Note>>
    {
        public int NoteId { get; set; }
        public string Text { get; set; }
    }

    public class UpdateNoteAuthorization : IAuthorize<UpdateNoteRequest>
    {
        private readonly INotesRepository _notesRepository;

        public UpdateNoteAuthorization(INotesRepository notesRepository)
        {
            _notesRepository = notesRepository;
        }

        public async Task<bool> Authorize(UpdateNoteRequest request)
        {
            var note = await _notesRepository.Get(request.NoteId);

            return note.UpdatedBy == request.User.GetIdentity();
        }
    }

    public class UpdateNoteValidator : AbstractValidator<UpdateNoteRequest>
    {
        private readonly INotesRepository _notesRepository;

        public UpdateNoteValidator(INotesRepository notesRepository)
        {
            _notesRepository = notesRepository;

            RuleFor(x => x.Text).NotEmpty();
            RuleFor(x => x).MustAsync(Exist).WithErrorCode(Result.NotPresent.StatusCode);
        }

        private async Task<bool> Exist(UpdateNoteRequest request, CancellationToken cancellationToken)
        {
            var note = await _notesRepository.Get(request.NoteId);
            return note != null;
        }
    }

    public class UpdateNoteHandler : IRequestHandler<UpdateNoteRequest, Result<Note>>
    {
        private readonly INotesRepository _notesRepository;

        public UpdateNoteHandler(INotesRepository notesRepository)
        {
            _notesRepository = notesRepository;
        }

        public async Task<Result<Note>> Handle(UpdateNoteRequest request, CancellationToken cancellationToken = default)
        {
            var note = await _notesRepository.Get(request.NoteId);

            note.UpdatedBy = request.User.GetIdentity();
            note.Text = request.Text;

            await _notesRepository.Update();
            return note;
        }
    }
}
