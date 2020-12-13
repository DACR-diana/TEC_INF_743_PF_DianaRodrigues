using Biblioteca.Core.Models.Books;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.Core.Services.Books
{
    public interface IBookService
    {
        Task<IEnumerable<Book>> GetAllWithCategoriesAndAuthor();
        Task<Book> GetWithCategoriesAndAuthorById(int id);
        Task<IEnumerable<Book>> GetAllWithCategoriesAndAuthorByAuthorId(int authorId);


        Task<Book> CreateBook(Book newBook);
        Task UpdateBook(Book bookToBeUpdated, Book book);
        //Task DeleteBook(Book book);
    }
}
