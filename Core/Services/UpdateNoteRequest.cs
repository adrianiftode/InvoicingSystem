using System.Security.Claims;

namespace Core.Services
{
    public class UpdateNoteRequest
    {
        public int NoteId { get; set; }
        public string Text { get; set; }
        public ClaimsPrincipal User { get; set; }
    }
}