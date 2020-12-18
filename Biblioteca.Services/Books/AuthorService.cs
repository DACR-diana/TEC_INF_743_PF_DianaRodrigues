using Biblioteca.Core;
using Biblioteca.Core.Models.Books;
using Biblioteca.Core.Services.Books;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.Services.Books
{
    public class AuthorService : IAuthorService
    {
        private readonly IUnitOfWork _unitOfWork;
        public AuthorService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Author>> GetAllAuthors()
        {
            return await _unitOfWork.Authors.GetAllAsync();
        }

        public async Task<IEnumerable<Author>> GetAllWithBooks()
        {
            return await _unitOfWork.Authors.GetAllWithBooksAsync();
        }

        public async Task<IEnumerable<Author>> GetWithBooksById(int id)
        {
            return await _unitOfWork.Authors.GetWithBooksByIdAsync(id);
        }
    }
}
