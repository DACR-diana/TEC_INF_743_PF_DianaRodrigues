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
    public class AuthorRepository : Repository<Author>, IAuthorRepository
    {
        private ApiDbContext ApiDbContext
        {
            get { return Context as ApiDbContext; }
        }

        public AuthorRepository(ApiDbContext context)
            : base(context)
        { }

        public async Task<IEnumerable<Author>> GetAllWithBooksAsync()
        {
            return await ApiDbContext.Authors
                .Include(a => a.BookAuthors)
                .ToListAsync();
        }

        public async  Task<IEnumerable<Author>> GetWithBooksByIdAsync(int id)
        {
            return await ApiDbContext.Authors
                .Include(a => a.BookAuthors)
                .Where(a => a.Id == id).ToListAsync();
        }

      
    }
}
