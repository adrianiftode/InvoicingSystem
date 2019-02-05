using System;


namespace Core
{
    public partial class Invoice
    {
        public static implicit operator ValueTuple<Invoice, Result>(Invoice invoice) => (invoice, Result.Success);

        public static implicit operator Invoice(ValueTuple<Invoice, Result> result) => result.Item1;
    }
}
