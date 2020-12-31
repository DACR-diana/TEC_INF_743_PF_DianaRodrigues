using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Biblioteca.WebApp.Models.Books
{
    public class AddViewModel
    {
        public List<Category> categories { get; set; }
        public List<Author> authors { get; set; }
        public List<Country> countries { get; set; }
    }
}
