using Biblioteca.Core.Models.Books;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.Core.Services.Books
{
    public interface IAuthorService
    {
        Task<IEnumerable<Author>> GetAllAuthors();
        Task<IEnumerable<Author>> GetAllWithBooks();
        Task<IEnumerable<Author>> GetWithBooksById(int id);


    }
}
