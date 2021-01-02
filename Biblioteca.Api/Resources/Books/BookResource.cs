using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Biblioteca.Api.Resources.Books
{
    public class BookResource
    {
        public int Id { get; set; }
        public int ISBN { get; set; }
        public string Title { get; set; }
        public CountryResource Country { get; set; }
        public ICollection<BookAuthorResource> BookAuthors { get; set; }
        public bool State { get; set; }
        public ICollection<BookCategoryResource> BookCategories { get; set; }
    }
}
