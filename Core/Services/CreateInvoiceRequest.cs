﻿namespace Core.Services
{
    public class CreateInvoiceRequest : Request
    {
        public string Identifier { get; set; }
        public decimal Amount { get; set; }
    }
}