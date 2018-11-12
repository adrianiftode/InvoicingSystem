using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Database
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<InvoicingContext>
    {
        public InvoicingContext CreateDbContext(string[] args)
        {
            var options = new DbContextOptionsBuilder<InvoicingContext>()
                 .UseSqlServer(@"Server=LAPTOP-LHBJ0CPO\SQLEXPRESS;Database=invoicing;Trusted_Connection=True;")
                 .Options;
            return new InvoicingContext(options);
        }
    }
}
