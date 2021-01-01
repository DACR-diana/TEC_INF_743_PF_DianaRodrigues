﻿using Biblioteca.Core.Models.Books;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.Core.Services.Books
{
    public interface IBookCategoryService
    {
        Task<BookCategory> CreateBookCategory(BookCategory newBookCategory);
        Task DeleteBookCategory(BookCategory bookCategory);
    }
}
