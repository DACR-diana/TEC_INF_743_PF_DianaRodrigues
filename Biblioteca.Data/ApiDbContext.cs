using Biblioteca.Core.Models;
using Biblioteca.Core.Models.Books;
using Biblioteca.Core.Models.Checkouts;
using Biblioteca.Core.Models.Users;
using Biblioteca.Data.Configurations;
using Biblioteca.Data.Configurations.Books;
using Biblioteca.Data.Configurations.Checkouts;
using Biblioteca.Data.Configurations.Users;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Biblioteca.Data
{
    public class ApiDbContext : DbContext
    {

        #region Books

        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<BookAuthor> BookAuthors { get; set; }
        public DbSet<BookCategory> BookCategories { get; set; }

        #endregion


        #region Users
        //public DbSet<User> Users { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Employee> Employees { get; set; }

        #endregion

        #region Checkouts

        public DbSet<Checkout> Checkouts { get; set; }
        public DbSet<CheckoutBook> CheckoutBooks { get; set; }

        #endregion


        public DbSet<Country> Countries { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Ticket> Tickets { get; set; }

        public ApiDbContext(DbContextOptions<ApiDbContext> options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder
                .ApplyConfiguration(new BookConfiguration());
            builder
                .ApplyConfiguration(new BookAuthorConfiguration());
            builder
                .ApplyConfiguration(new BookCategoryConfiguration());
            builder
                .ApplyConfiguration(new ClientConfiguration());
            builder
                .ApplyConfiguration(new CheckoutConfiguration());
            builder
               .ApplyConfiguration(new CheckoutBookConfiguration());
            builder
               .ApplyConfiguration(new TicketConfiguration());
        }
}
}
