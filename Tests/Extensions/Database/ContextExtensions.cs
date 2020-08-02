using Database;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Tests.Extensions.Database
{
    internal static class ContextExtensions
    {
        public static async Task<int> GetNextInvoiceId(this InvoicingContext context)
            => await context.Invoices.Select(c => c.InvoiceId).MaxAsync() + 1; //see this why it needs the setup like this https://github.com/aspnet/EntityFrameworkCore/issues/12371
    }
}
