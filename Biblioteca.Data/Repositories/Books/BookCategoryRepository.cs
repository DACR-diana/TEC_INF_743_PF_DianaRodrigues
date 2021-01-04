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
    public class BookCategoryRepository : Repository<BookCategory>, IBookCategoryRepository
    {
        private ApiDbContext ApiDbContext
        {
            get { return Context as ApiDbContext; }
        }

        public BookCategoryRepository(ApiDbContext context)
            : base(context)
        { }

        public async Task<IEnumerable<BookCategory>> GetBookCategoryByIdAsync(int id)
        {
            return await ApiDbContext.BookCategories
                .Where(m => m.BookId == id).ToListAsync();
        }
    }
}
