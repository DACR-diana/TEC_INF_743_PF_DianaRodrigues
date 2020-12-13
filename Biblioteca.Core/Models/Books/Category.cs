using System;
using System.Collections.Generic;
using System.Text;

namespace Biblioteca.Core.Models.Books
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Book> Books { get; set; }


        public Category()
        {
            this.Books = new HashSet<Book>();
        }
    }
}
