namespace Database.Entities
{
    public class Note
    {
        public int NoteId { get; set; }
        public string Text { get; set; }
        public int InvoiceId { get; set; }
        public int UpdatedByUserId { get; set; }
        public User UpdatedBy { get; set; }
    }
}