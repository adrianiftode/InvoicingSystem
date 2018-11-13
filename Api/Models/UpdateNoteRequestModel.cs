using System.ComponentModel.DataAnnotations;

namespace Api.Models
{
    public class UpdateNoteRequestModel
    {
        public int NoteId { get; set; }
        [Required]
        public string Text { get; set; }
    }
}