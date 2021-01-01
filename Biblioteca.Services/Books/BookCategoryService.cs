using Biblioteca.Core;
using Biblioteca.Core.Models.Books;
using Biblioteca.Core.Services.Books;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.Services.Books
{
    public class BookCategoryService : IBookCategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        public BookCategoryService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }
        public async Task<BookCategory> CreateBookCategory(BookCategory newBookCategory)
        {
            await _unitOfWork.BookCategories.AddAsync(newBookCategory);
            await _unitOfWork.CommitAsync();
            return newBookCategory;
        }


        public async Task DeleteBookCategory(BookCategory bookCategory)
        {
            _unitOfWork.BookCategories.Remove(bookCategory);
            await _unitOfWork.CommitAsync();
        }
    }
}
