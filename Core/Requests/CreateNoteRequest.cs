﻿namespace Core
{
    public class CreateNoteRequest : Request
    {
        public int InvoiceId { get; set; }
        public string Text { get; set; }
    }
}