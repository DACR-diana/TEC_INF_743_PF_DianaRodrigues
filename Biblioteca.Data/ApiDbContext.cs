﻿using Biblioteca.Core.Models;
using Biblioteca.Core.Models.Books;
using Biblioteca.Core.Models.Users;
using Biblioteca.Data.Configurations;
using Biblioteca.Data.Configurations.Books;
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

        #endregion


        #region Users
        public DbSet<Client> Clients { get; set; }
        public DbSet<Employee> Employees { get; set; }

        #endregion


        public DbSet<Checkout> Checkouts { get; set; }
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
                .ApplyConfiguration(new ClientConfiguration());
            builder
                .ApplyConfiguration(new CheckoutConfiguration());
            builder
               .ApplyConfiguration(new TicketConfiguration());
        }
    }
}
