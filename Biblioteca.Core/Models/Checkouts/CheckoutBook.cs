using Biblioteca.Core.Models.Books;
using System;
using System.Collections.Generic;
using System.Text;

namespace Biblioteca.Core.Models.Checkouts
{
    public class CheckoutBook
    {
        public int BookId { get; set; }
        public Book Book { get; set; }
        public int CheckoutId { get; set; }
        public Checkout Checkout { get; set; }
    }
}
