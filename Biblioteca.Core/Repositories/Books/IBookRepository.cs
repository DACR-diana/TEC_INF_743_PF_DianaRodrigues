using Biblioteca.Core.Models.Books;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.Core.Repositories.Books
{
    public interface IBookRepository : IRepository<Book>
    {
        Task<IEnumerable<Book>> GetAllWithCategoriesAndAuthorAsync();
        Task<IEnumerable<Book>> GetAllByStateAsync(bool state);
        Task<List<Book>> GetByISBNAsync(int ISBN);
        Task<Book> GetWithCategoriesAndAuthorByIdAsync(int id);
    }
}
