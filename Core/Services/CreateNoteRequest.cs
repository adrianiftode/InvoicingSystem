using System.Security.Claims;

namespace Core.Services
{
    public class CreateNoteRequest
    {
        public int InvoiceId { get; set; }
        public string Text { get; set; }
        public ClaimsPrincipal User { get; set; }
    }
}