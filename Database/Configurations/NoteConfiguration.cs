using Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Database.Configurations
{
    public class NoteConfiguration : IEntityTypeConfiguration<Note>
    {
        public void Configure(EntityTypeBuilder<Note> builder)
        {
            builder.Property(c => c.UpdatedBy).IsRequired();
            builder.Property(c => c.Text).IsRequired().HasMaxLength(1000);
        }
    }
}
