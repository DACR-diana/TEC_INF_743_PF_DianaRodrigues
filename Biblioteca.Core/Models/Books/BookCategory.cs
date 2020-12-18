using System;
using System.Collections.Generic;
using System.Text;

namespace Biblioteca.Core.Models.Books
{
    public class BookCategory
    {
        public int BookId { get; set; }
        public Book Book { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
