using Biblioteca.Core.Models.Books;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.Core.Services.Books
{
    public interface IBookAuthorService
    {
        Task<BookAuthor> CreateBookAuthor(BookAuthor newBookAuthor);
        Task DeleteBookAuthor(BookAuthor bookAuthor);
        Task<IEnumerable<BookAuthor>> GetBookAuthorById(int bookId);
    }
}
