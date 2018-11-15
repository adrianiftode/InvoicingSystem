using Core.Repositories;
using System.Threading.Tasks;


namespace Core
{
    public class NotesService : INotesService
    {
        private readonly INotesRepository _notesRepository;
        private readonly IInvoicesRepository _invoicesRepository;

        public NotesService(INotesRepository notesRepository, IInvoicesRepository invoicesRepository)
        {
            _notesRepository = notesRepository;
            _invoicesRepository = invoicesRepository;
        }
        public async Task<Note> Get(int id)
        {
            var note = await _notesRepository.Get(id);
            return note;
        }

        public async Task<Result<Note>> Create(CreateNoteRequest request)
        {
            var invoice = await _invoicesRepository.Get(request.InvoiceId);

            if (invoice == null)
            {
                return Result.Error("Note could not be created because the targeted invoice is not present.");
            }

            var note = invoice.AddNote(request.Text, request.User.GetIdentity());

            await _notesRepository.Create(note);
            return note;
        }

        public async Task<Result<Note>> Update(UpdateNoteRequest request)
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