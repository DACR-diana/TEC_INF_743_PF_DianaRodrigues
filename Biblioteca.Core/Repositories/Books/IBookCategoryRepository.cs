using Biblioteca.Core.Models.Books;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.Core.Repositories.Books
{
    public interface IBookCategoryRepository : IRepository<BookCategory>
    {
        Task<IEnumerable<BookCategory>> GetBookCategoryByIdAsync(int id);
    }
}
