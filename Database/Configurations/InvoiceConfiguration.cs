using Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Database.Configurations
{
    public class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
    {
        public void Configure(EntityTypeBuilder<Invoice> builder)
        {
            builder.Property(c => c.UpdatedBy).IsRequired();
            builder.Property(c => c.Identifier).IsRequired();
            builder.Property(c => c.Amount).IsRequired().HasColumnType("decimal(9,2)");
            builder.Property(c => c.Identifier).IsRequired().HasMaxLength(100);
        }
    }
}
