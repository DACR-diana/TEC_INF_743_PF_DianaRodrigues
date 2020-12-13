using Biblioteca.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Biblioteca.Data.Configurations
{
    class TicketConfiguration : IEntityTypeConfiguration<Ticket>
    {
        public void Configure(EntityTypeBuilder<Ticket> builder)
        {
            builder
                .HasKey(m => m.Id);

            builder
                .Property(m => m.Id)
                .UseIdentityColumn();

            builder
               .Property(m => m.Price).HasColumnType("decimal")
               .IsRequired();

            builder
              .Property(m => m.Date)
              .IsRequired();

            

            builder
                     .HasOne(m => m.Payment)
                     .WithMany(a => a.Tickets)
                     .HasForeignKey(m => m.PaymentId);

            builder
                    .HasOne(m => m.Checkout)
                    .WithMany(a => a.Tickets)
                    .HasForeignKey(m => m.CheckoutId);
        }
    }
}
