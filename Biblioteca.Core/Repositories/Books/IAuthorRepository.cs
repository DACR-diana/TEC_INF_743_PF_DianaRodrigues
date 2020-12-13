using Biblioteca.Core.Models.Books;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.Core.Repositories.Books
{
    public interface IAuthorRepository : IRepository<Author>
    {
        Task<IEnumerable<Author>> GetAllWithBooksAsync();
        Task<Author> GetWithBooksByIdAsync(int id);
    }
}
