using System.Security.Claims;

namespace Core.Services
{
    public class UpdateInvoiceRequest
    {
        public int InvoiceId { get; set; }
        public string Identifier { get; set; }
        public decimal Amount { get; set; }
        public ClaimsPrincipal User { get; set; }
    }
}