using Biblioteca.Core.Models.Books;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Biblioteca.Data.Configurations.Books
{
    public class BookConfiguration : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder
                .HasKey(m => m.Id);

            builder
                .Property(m => m.Id)
                .UseIdentityColumn();
            builder
               .Property(m => m.ISBN)
               .IsRequired()
               .HasMaxLength(5);

            builder
                .Property(m => m.Title)
                .IsRequired()
                .HasMaxLength(50);
            builder
                .HasOne(m => m.Country)
                .WithMany(a => a.Books)
                .HasForeignKey(m => m.CountryId);


        }
    }

}
