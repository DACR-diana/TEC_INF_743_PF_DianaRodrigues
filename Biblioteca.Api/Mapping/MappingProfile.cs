using AutoMapper;
using Biblioteca.Api.Resources.Books;
using Biblioteca.Core.Models.Books;
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

            // Resource to Domain
            CreateMap<AuthorResource, Author>();
            CreateMap<CategoryResource, Category>();
            CreateMap<BookResource, Book>();

        }
    }
}
