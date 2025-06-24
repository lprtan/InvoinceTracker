using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InvoinceModule.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace InvoinceModule.Infrastructure.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<InvoiceHeader> InvoiceHeaders { get; set; }
        public DbSet<InvoiceDetail> InvoiceDetails { get; set; }
        public DbSet<InvoiceNumberSequences> InvoiceNumberSequences { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<InvoiceHeader>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.HasIndex(e => e.InvoiceNumber).IsUnique();

                entity.Property(e => e.InvoiceNumber)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.CustomerName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.CustomerEmail)
                    .HasMaxLength(100);

                entity.Property(e => e.SenderTitle)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.ReceiverTitle)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasMany(e => e.Details)
                      .WithOne(d => d.InvoiceHeader)
                      .HasForeignKey(d => d.InvoiceHeaderId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<InvoiceDetail>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.ProductName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Quantity)
                    .IsRequired();

                entity.Property(e => e.UnitPrice)
                    .HasColumnType("decimal(18,2)")
                    .IsRequired();
            });

            modelBuilder.Entity<InvoiceNumberSequences>().HasData(
                new InvoiceNumberSequences
                {
                    Id = 1,
                    LastNumber = 1
                }
            );

            // InvoiceHeader Seed
            modelBuilder.Entity<InvoiceHeader>().HasData(
                new InvoiceHeader
                {
                    Id = 4,
                    InvoiceNumber = "SVS20255",
                    InvoiceDate = new DateTime(2025, 6, 23, 20, 52, 48, 173),
                    CustomerName = "Alperen Tan",
                    CustomerEmail = "lprntan@gmail.com",
                    IsProcessed = true,
                    IsMailSent = false,
                    ReceiverTitle = "Alıcı Firma",
                    SenderTitle = "Gönderici Firma"
                }
            );

            // InvoiceDetail Seed
            modelBuilder.Entity<InvoiceDetail>().HasData(
                new InvoiceDetail
                {
                    Id = 5,
                    InvoiceHeaderId = 4,
                    ProductName = "Süt",
                    Quantity = 10,
                    UnitPrice = 8.00m
                },
                new InvoiceDetail
                {
                    Id = 6,
                    InvoiceHeaderId = 4,
                    ProductName = "Kalem",
                    Quantity = 100,
                    UnitPrice = 3.00m
                },
                new InvoiceDetail
                {
                    Id = 7,
                    InvoiceHeaderId = 4,
                    ProductName = "Kahve",
                    Quantity = 18,
                    UnitPrice = 15.00m
                }
            );

            base.OnModelCreating(modelBuilder);
        }
    }
}
