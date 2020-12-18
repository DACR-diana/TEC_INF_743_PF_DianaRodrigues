using Biblioteca.Core.Models.Checkouts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Biblioteca.Data.Configurations.Checkouts
{
    public class CheckoutBookConfiguration : IEntityTypeConfiguration<CheckoutBook>
    {
        public void Configure(EntityTypeBuilder<CheckoutBook> builder)
        {
            builder
                .HasKey(cb => new { cb.BookId, cb.CheckoutId });

            builder
            .HasOne(cb => cb.Book)
            .WithMany(b => b.CheckoutBooks)
            .HasForeignKey(cb => cb.BookId);

            builder
             .HasOne(cb => cb.Checkout)
             .WithMany(c => c.CheckoutBooks)
             .HasForeignKey(cb => cb.CheckoutId);
        }
    }
}
