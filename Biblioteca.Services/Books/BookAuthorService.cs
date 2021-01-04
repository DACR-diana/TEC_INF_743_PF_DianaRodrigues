using Biblioteca.Core;
using Biblioteca.Core.Models.Books;
using Biblioteca.Core.Services.Books;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.Services.Books
{
    public class BookAuthorService : IBookAuthorService
    {
        private readonly IUnitOfWork _unitOfWork;
        public BookAuthorService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }
        public async Task<BookAuthor> CreateBookAuthor(BookAuthor newBookAuhtor)
        {
            await _unitOfWork.BookAuthors.AddAsync(newBookAuhtor);
            await _unitOfWork.CommitAsync();
            return newBookAuhtor;
        }
        public async Task<IEnumerable<BookAuthor>> GetBookAuthorById(int bookId)
        {
            return await _unitOfWork.BookAuthors.GetBookAuthorByIdAsync(bookId);
        }

        public async Task DeleteBookAuthor(BookAuthor bookAuthor)
        {
            _unitOfWork.BookAuthors.Remove(bookAuthor);
            await _unitOfWork.CommitAsync();
        }
    }
}
