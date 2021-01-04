using Biblioteca.Core.Models.Books;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.Core.Repositories.Books
{
    public interface IBookAuthorRepository : IRepository<BookAuthor>
    {
        Task<IEnumerable<BookAuthor>> GetBookAuthorByIdAsync(int id);
    }
}
