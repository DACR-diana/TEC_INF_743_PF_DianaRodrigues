using AutoMapper;
using Biblioteca.Api.Resources;
using Biblioteca.Api.Resources.Books;
using Biblioteca.Api.Resources.Checkouts;
using Biblioteca.Api.Resources.Users;
using Biblioteca.Core.Models;
using Biblioteca.Core.Models.Books;
using Biblioteca.Core.Models.Checkouts;
using Biblioteca.Core.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Biblioteca.Api.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Domain to Resource
            CreateMap<Author, AuthorResource>();
            CreateMap<Category, CategoryResource>();
            CreateMap<Book, BookResource>();
            CreateMap<BookAuthor, BookAuthorResource>();
            CreateMap<BookCategory, BookCategoryResource>();
            CreateMap<Checkout, CheckoutResource>();
            CreateMap<CheckoutBook, CheckoutBookResource>();
            CreateMap<User, UserResource>();
            CreateMap<Employee, EmployeeResource>();
            CreateMap<Client, ClientResource>();
            CreateMap<Country, CountryResource>();
            CreateMap<Payment, PaymentResource>();
            CreateMap<Ticket, TicketResource>();


            // Resource to Domain
            CreateMap<AuthorResource, Author>();
            CreateMap<CategoryResource, Category>();
            CreateMap<BookResource, Book>();
            CreateMap<BookAuthorResource, BookAuthor>();
            CreateMap<BookCategoryResource, BookCategory>();
            CreateMap<SaveBookResource, Book>();
            CreateMap<CheckoutResource, Checkout>();
            CreateMap<SaveCheckoutResource, Checkout>();
            CreateMap<CheckoutBookResource, CheckoutBook>();
            CreateMap<UserResource, User>();
            CreateMap<EmployeeResource, Employee>();
            CreateMap<ClientResource, Client>();
            CreateMap<SaveClientResource, Client>();
            CreateMap<CountryResource, Country>();
            CreateMap<PaymentResource, Payment>();
            CreateMap<SaveTicketResource, Ticket>();
            CreateMap<TicketResource, Ticket>();

        }
    }
}
