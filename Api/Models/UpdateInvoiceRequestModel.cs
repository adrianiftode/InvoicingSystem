using System.ComponentModel.DataAnnotations;

namespace Api.Models
{
    public class UpdateInvoiceRequestModel
    {
        [Required]
        public string Identifier { get; set; }
        public decimal Amount { get; set; }
        public int InvoiceId { get; set; }
    }
}