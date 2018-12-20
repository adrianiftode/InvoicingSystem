using System.Threading;
using System.Threading.Tasks;
using Core.Repositories;
using MediatR;

namespace Core
{
    public class CreateNoteRequest : Request, IRequest<Result<Note>>
    {
        public int InvoiceId { get; set; }
        public string Text { get; set; }
    }

    public class CreateNoteHandler : IRequestHandler<CreateNoteRequest, Result<Note>>
    {
        private readonly INotesRepository _notesRepository;
        private readonly IInvoicesRepository _invoicesRepository;

        public CreateNoteHandler(INotesRepository notesRepository, IInvoicesRepository invoicesRepository)
        {
            _notesRepository = notesRepository;
            _invoicesRepository = invoicesRepository;
        }

        public async Task<Result<Note>> Handle(CreateNoteRequest request, CancellationToken cancellationToken = default(CancellationToken))
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
    }
}
