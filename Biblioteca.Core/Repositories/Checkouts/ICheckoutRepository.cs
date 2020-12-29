using Biblioteca.Core.Models.Books;
using Biblioteca.Core.Models.Checkouts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.Core.Repositories.Checkouts
{
    public interface ICheckoutRepository : IRepository<Checkout>
    {

        Checkout GetWithCheckoutBooksByFilter(string[] filters, string[] filters_text);
        Checkout CreateCheckout(Checkout newCheckout);
        Checkout UpdateCheckout(Checkout checkoutToBeUpdated);
    }
}
