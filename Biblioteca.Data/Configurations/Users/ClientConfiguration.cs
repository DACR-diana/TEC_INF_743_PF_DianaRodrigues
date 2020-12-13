using Biblioteca.Core.Models.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Biblioteca.Data.Configurations.Users
{
    class ClientConfiguration : IEntityTypeConfiguration<Client>
    {
        public void Configure(EntityTypeBuilder<Client> builder)
        {
            builder
                .HasKey(m => m.Id);

            builder
                .Property(m => m.Id)
                .UseIdentityColumn();
            builder
               .Property(m => m.Name)
               .IsRequired()
               .HasMaxLength(50);

            builder
                .Property(m => m.NIF)
                .IsRequired()
                .HasMaxLength(9);

            builder
             .Property(m => m.Email)
             .IsRequired()
             .HasMaxLength(100);

            builder
            .Property(m => m.Registration)
            .IsRequired();
        }
    }
}
