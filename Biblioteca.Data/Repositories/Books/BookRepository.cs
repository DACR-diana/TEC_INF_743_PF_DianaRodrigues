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
    public class BookRepository : Repository<Book>, IBookRepository
    {
        private ApiDbContext ApiDbContext
        {
            get { return Context as ApiDbContext; }
        }

        public BookRepository(ApiDbContext context)
            : base(context)
        { }

        public async Task<IEnumerable<Book>> GetAllWithCategoriesAndAuthorAsync()
        {
            return await ApiDbContext.Books
                .Include(m => m.Category)
                .Include(m => m.Author)
                .ToListAsync();
        }

        public async Task<Book> GetWithCategoriesAndAuthorByIdAsync(int id)
        {
            return await ApiDbContext.Books
              .Include(m => m.Category)
                .Include(m => m.Author)
                .SingleOrDefaultAsync(m => m.Id == id); ;
        }

        public async Task<IEnumerable<Book>> GetAllWithCategoriesAndAuthorByAuthorIdAsync(int authorId)
        {
            return await ApiDbContext.Books
               .Include(m => m.Category)
                .Include(m => m.Author)
                .Where(m => m.Author.Id == authorId)
                .ToListAsync();
        }


    }
}
