using Biblioteca.Api.Resources.Books;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Biblioteca.Api.Resources.Checkouts
{
    public class CheckoutBookResource
    {
        public BookResource Book { get; set; }
        public CheckoutResource Checkout { get; set; }
    }
}
