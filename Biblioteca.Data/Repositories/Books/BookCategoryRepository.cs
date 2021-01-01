using Biblioteca.Core.Models.Books;
using Biblioteca.Core.Repositories.Books;
using System;
using System.Collections.Generic;
using System.Text;

namespace Biblioteca.Data.Repositories.Books
{
    public class BookCategoryRepository : Repository<BookCategory>, IBookCategoryRepository
    {
        private ApiDbContext ApiDbContext
        {
            get { return Context as ApiDbContext; }
        }

        public BookCategoryRepository(ApiDbContext context)
            : base(context)
        { }

    }
}
