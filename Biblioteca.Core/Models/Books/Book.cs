using Biblioteca.Core.Models.Books;
using System;
using System.Collections.Generic;
using System.Text;

namespace Biblioteca.Core.Models.Books
{
    public class Book
    {
        public int Id { get; set; }
        public int ISBN { get; set; }
        public string Title { get; set; }
        public int CountryId { get; set; }
        public int AuthorId { get; set; }
        public Country Country { get; set; }
        public Author Author { get; set; }
        public bool State { get; set; }
        public ICollection<Category> Category { get; set; }

        public Book()
        {
            this.Category = new HashSet<Category>();
        }
    }
}
