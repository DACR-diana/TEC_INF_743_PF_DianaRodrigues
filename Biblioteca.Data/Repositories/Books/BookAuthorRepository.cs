using Biblioteca.Core.Models.Books;
using Biblioteca.Core.Repositories.Books;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<IEnumerable<BookAuthor>> GetBookAuthorByIdAsync(int id)
        {
            return await ApiDbContext.BookAuthors
                .Where(m => m.BookId == id).ToListAsync();
        }

    }
}
