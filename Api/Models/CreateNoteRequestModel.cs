using System.ComponentModel.DataAnnotations;

namespace Api.Models
{
    public class CreateNoteRequestModel
    {
        [Required]
        public string Text { get; set; }
        public int InvoiceId { get; set; }
    }
}