using Biblioteca.Core.Models.Books;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.Core.Services.Books
{
    public interface IAuthorService
    {
        Task<Author> GetWithBooksById(int id);
        Task<IEnumerable<Author>> GetAllWithBooksByBooksId(int bookId);

    }
}
