using Biblioteca.Core;
using Biblioteca.Core.Models.Books;
using Biblioteca.Core.Services.Books;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.Services.Books
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Category>> GetAllCategories()
        {
            return await _unitOfWork.Categories.GetAllAsync();
        }

        public async Task<IEnumerable<Category>> GetAllWithBooks()
        {
            return await _unitOfWork.Categories.GetAllWithBooksAsync();
        }

        public async Task<IEnumerable<Category>> GetWithBooksById(int id)
        {
            return await _unitOfWork.Categories.GetWithBooksByIdAsync(id);
        }


    }
}
