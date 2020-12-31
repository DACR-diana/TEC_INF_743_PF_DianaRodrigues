using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Biblioteca.Api.Resources.Books
{
    public class SaveBookResource
    {
        public int ISBN { get; set; }
        public string Title { get; set; }
        public int CountryId { get; set; }
        public ICollection<AuthorResource> Authors { get; set; }
        public bool State { get; set; }
        public ICollection<CategoryResource> Categories { get; set; }
    }
}
