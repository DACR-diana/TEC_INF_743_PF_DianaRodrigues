using Biblioteca.Core.Models.Books;
using Biblioteca.Core.Repositories.Books;
using System;
using System.Collections.Generic;
using System.Text;

namespace Biblioteca.Data.Repositories.Books
{
    public class BookAuthorRepository : Repository<BookAuthor>, IBookAuthorRepository
    {
        private ApiDbContext ApiDbContext
        {
            get { return Context as ApiDbContext; }
        }

        public BookAuthorRepository(ApiDbContext context)
            : base(context)
        { }

    }
}
