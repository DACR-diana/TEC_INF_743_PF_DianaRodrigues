using Biblioteca.Core.Models;
using Biblioteca.Core.Models.Checkouts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Biblioteca.Data.Configurations.Checkouts
{
    class CheckoutConfiguration : IEntityTypeConfiguration<Checkout>
    {
        public void Configure(EntityTypeBuilder<Checkout> builder)
        {
            builder
                .HasKey(m => m.Id);

            builder
                .Property(m => m.Id)
                .UseIdentityColumn();
            builder
               .Property(m => m.Date)
               .IsRequired();
            builder
              .Property(m => m.ExpectedDate)
              .IsRequired();

            builder
               .HasOne(m => m.Client)
               .WithMany(a => a.Checkouts)
               .HasForeignKey(m => m.ClientId);
        }
    }
}
