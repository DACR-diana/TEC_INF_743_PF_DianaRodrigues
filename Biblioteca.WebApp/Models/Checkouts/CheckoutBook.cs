using Biblioteca.WebApp.Models.Books;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Biblioteca.WebApp.Models.Checkouts
{
    public class CheckoutBook
    {
        public Book Book { get; set; }
        public Checkout Checkout { get; set; }
    }
}
