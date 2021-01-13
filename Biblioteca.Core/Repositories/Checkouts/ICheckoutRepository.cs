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
        List<Checkout> GetWithCheckoutBooksByFilter(string[] filters, string[] filters_text);
        List<Checkout> GetWithCheckoutBooksByFilterByState(string[] filters, string[] filters_text,bool state);
        List<Checkout> GetExpiredCheckouts();

        Checkout GetExpiredCheckoutById(int checkoutId);
        Checkout CreateCheckout(Checkout newCheckout);
        Checkout UpdateCheckout(Checkout checkoutToBeUpdated);
    }
}
