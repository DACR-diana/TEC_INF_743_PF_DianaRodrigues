using Biblioteca.Core.Models.Books;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.Core.Services.Books
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetAllWithBooks();
        Task<Category> GetWithBooksById(int id);
    }
}
