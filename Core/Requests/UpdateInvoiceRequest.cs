namespace Core
{
    public class UpdateInvoiceRequest : Request
    {
        public int InvoiceId { get; set; }
        public string Identifier { get; set; }
        public decimal Amount { get; set; }
    }
}
