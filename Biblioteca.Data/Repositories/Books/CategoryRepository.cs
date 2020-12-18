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
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private ApiDbContext ApiDbContext
        {
            get { return Context as ApiDbContext; }
        }

        public CategoryRepository(ApiDbContext context)
            : base(context)
        { }

        public async Task<IEnumerable<Category>> GetAllWithBooksAsync()
        {
            return await ApiDbContext.Categories
                .Include(a => a.BookCategories)
                .ToListAsync();
        }

        public async Task<IEnumerable<Category>> GetWithBooksByIdAsync(int id)
        {
            return await ApiDbContext.Categories
                .Include(a => a.BookCategories)
                .Where(a => a.Id == id).ToListAsync();
        }

    }
}
