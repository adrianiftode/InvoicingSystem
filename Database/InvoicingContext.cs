using Database.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Database
{
    public class InvoicingContext : DbContext
    {
        public InvoicingContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Invoice> Invoices { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            RegisterConfigurations(modelBuilder);
            Seed(modelBuilder);
        }

        private static void Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                new User { Identity = "1", UserId = 1 },
                new User { Identity = "2", UserId = 2 },
                new User { Identity = "3", UserId = 3 },
                new User { Identity = "4", UserId = 4 }
            );
            modelBuilder.Entity<Note>().HasData(new Note
            {
                NoteId = 1,
                Text = "Invoice should be paid soon!",
                UpdatedByUserId = 1,
                InvoiceId = 1
            });
            modelBuilder.Entity<Invoice>().HasData(
                new Invoice
                {
                    InvoiceId = 1,
                    Amount = 150.05m,
                    Identifier = "INV-001",
                    UpdatedByUserId = 1
                },
                new Invoice
                {
                    InvoiceId = 2,
                    Amount = 150.05m,
                    Identifier = "INV-002",
                    UpdatedByUserId = 2
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
