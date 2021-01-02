using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Biblioteca.WebApp.Models.Books
{
    public class Book
    {
        public int Id { get; set; }
        public int ISBN { get; set; }
        public string Title { get; set; }
        public int CountryId { get; set; }
        public Country Country { get; set; }
        public ICollection<Author> Authors { get; set; }
        public ICollection<BookAuthor> BookAuthors { get; set; }
        public bool State { get; set; }
        public ICollection<BookCategory> BookCategories { get; set; }
        public ICollection<Category> Categories { get; set; }
    }
}
