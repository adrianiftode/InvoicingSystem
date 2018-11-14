namespace Core.Services
{
    public class UpdateNoteRequest : Request
    {
        public int NoteId { get; set; }
        public string Text { get; set; }
    }
}
