using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using Core;

namespace Database
{
    public class InvoicingContext : DbContext
    {
        public InvoicingContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Core.Invoice> Invoices { get; set; }
        public DbSet<Core.Note> Notes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            RegisterConfigurations(modelBuilder);
            Seed(modelBuilder);
        }

        private static void Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Note>().HasData(new Note
            {
                NoteId = 1,
                Text = "Invoice should be paid soon!",
                UpdatedBy = "1",
                InvoiceId = 1
            });
            modelBuilder.Entity<Invoice>().HasData(
                new Invoice
                {
                    InvoiceId = 1,
                    Amount = 150.05m,
                    Identifier = "INV-001",
                    UpdatedBy = "1"
                },
                new Invoice
                {
                    InvoiceId = 2,
                    Amount = 150.05m,
                    Identifier = "INV-002",
                    UpdatedBy = "1"
                }
            );

        }

        private static void RegisterConfigurations(ModelBuilder modelBuilder)
        {
            var typesToRegister = typeof(InvoicingContext).Assembly.GetTypes()
                .Where(t => t.GetInterfaces().Any(gi =>
                    gi.IsGenericType && gi.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>))).ToList();


            foreach (var type in typesToRegister)
            {
                dynamic configurationInstance = Activator.CreateInstance(type);
                modelBuilder.ApplyConfiguration(configurationInstance);
            }
        }
    }
}
