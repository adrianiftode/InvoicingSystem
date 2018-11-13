using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Core.Services
{
    public interface IInvoicesService
    {
        Task<Invoice> Get(int id);
        Task<IReadOnlyCollection<Note>> GetNotesBy(int invoiceId);
        Task<AddNoteResponse> AddNote(AddNoteRequest note);
    }

    public class AddNoteResponse
    {
        public bool Added { get; set; }
        public Note Item { get; set; }
    }

    public class AddNoteRequest
    {
        public string Text { get; set; }
        public int InvoiceId { get; set; }
        public ClaimsPrincipal User { get; set; }
    }
}