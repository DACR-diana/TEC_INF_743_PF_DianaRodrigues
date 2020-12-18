using System;
using System.Collections.Generic;
using System.Text;

namespace Biblioteca.Core.Models.Books
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<BookCategory> BookCategories { get; set; }

    }
}
