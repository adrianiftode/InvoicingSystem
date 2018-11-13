using System.ComponentModel.DataAnnotations;

namespace Api.Models
{
    public class CreateInvoiceRequestModel
    {
        [Required]
        public string Identifier { get; set; }
        public decimal Amount { get; set; }
    }
}