using Biblioteca.Core.Models.Books;
using Biblioteca.Core.Models.Checkouts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.Core.Services.Checkouts
{
    public interface ICheckoutService
    {
        List<Checkout> GetWithCheckoutBooksById(int Id);
        List<Checkout> GetWithCheckoutBooksByClientId(int Id);
        List<Checkout> GetWithCheckoutBooksByClientIdAndState(int Id, bool state);
        List<Checkout> GetExpiredCheckouts();
        int GetClientCount();
        Checkout GetExpiredCheckoutById(int checkoutId);
        Checkout CreateCheckout(Checkout newCheckout);
        Checkout UpdateCheckout(Checkout checkoutToBeUpdated);
    }
}
