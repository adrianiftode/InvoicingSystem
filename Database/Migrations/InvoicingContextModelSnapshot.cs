﻿// <auto-generated />

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Database.Migrations
{
    [DbContext(typeof(InvoicingContext))]
    partial class InvoicingContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Core.Invoice", b =>
                {
                    b.Property<int>("InvoiceId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(9,2)");

                    b.Property<string>("Identifier")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<string>("UpdatedBy")
                        .IsRequired();

                    b.HasKey("InvoiceId");

                    b.ToTable("Invoices");

                    b.HasData(
                        new { InvoiceId = 1, Amount = 150.05m, Identifier = "INV-001", UpdatedBy = "1" },
                        new { InvoiceId = 2, Amount = 150.05m, Identifier = "INV-002", UpdatedBy = "1" }
                    );
                });

            modelBuilder.Entity("Core.Note", b =>
                {
                    b.Property<int>("NoteId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("InvoiceId");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasMaxLength(1000);

                    b.Property<string>("UpdatedBy")
                        .IsRequired();

                    b.HasKey("NoteId");

                    b.HasIndex("InvoiceId");

                    b.ToTable("Notes");

                    b.HasData(
                        new { NoteId = 1, InvoiceId = 1, Text = "Invoice should be paid soon!", UpdatedBy = "1" }
                    );
                });

            modelBuilder.Entity("Core.Note", b =>
                {
                    b.HasOne("Core.Invoice")
                        .WithMany("Notes")
                        .HasForeignKey("InvoiceId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
