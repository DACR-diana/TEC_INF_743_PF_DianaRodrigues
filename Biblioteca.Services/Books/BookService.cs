using Biblioteca.Core;
using Biblioteca.Core.Models.Books;
using Biblioteca.Core.Services.Books;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.Services.Books
{
    public class BookService : IBookService
    {
        private readonly IUnitOfWork _unitOfWork;
        public BookService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Book>> GetAllBooks()
        {
            return await _unitOfWork.Books.GetAllAsync();
        }

        public async Task<IEnumerable<Book>> GetAllWithCategoriesAndAuthor()
        {
            return await _unitOfWork.Books.GetAllWithCategoriesAndAuthorAsync();
        }

        public async Task<Book> GetWithCategoriesAndAuthorById(int id)
        {
            return await _unitOfWork.Books.GetWithCategoriesAndAuthorByIdAsync(id);
        }

        public async Task<IEnumerable<Book>> GetAllByState(bool state)
        {
            return await _unitOfWork.Books.GetAllByStateAsync(state);
        }

        public async Task<Book> CreateBook(Book newBook)
        {
            await _unitOfWork.Books.AddAsync(newBook);
            await _unitOfWork.CommitAsync();
            return newBook;
        }


        public async Task UpdateBook(Book bookToBeUpdated, Book book)
        {
            bookToBeUpdated.CountryId = book.CountryId;
            bookToBeUpdated.Title = book.Title;
            bookToBeUpdated.State = book.State;
            bookToBeUpdated.ISBN = book.ISBN;

            _unitOfWork.Books.UpdateAsync(bookToBeUpdated);
            await _unitOfWork.CommitAsync();
        }
    }
}
