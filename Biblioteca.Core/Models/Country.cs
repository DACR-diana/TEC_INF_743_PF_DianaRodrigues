using Biblioteca.Core.Models.Books;
using System;
using System.Collections.Generic;
using System.Text;

namespace Biblioteca.Core.Models
{
    public class Country
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Book> Books { get; set; }
    }
}
