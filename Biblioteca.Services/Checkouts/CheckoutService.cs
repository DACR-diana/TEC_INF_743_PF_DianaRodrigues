using Biblioteca.Core;
using Biblioteca.Core.Models.Books;
using Biblioteca.Core.Models.Checkouts;
using Biblioteca.Core.Services.Checkouts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.Services
{
    public class CheckoutService : ICheckoutService
    {
        private readonly IUnitOfWork _unitOfWork;
        public CheckoutService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public Checkout GetWithCheckoutBooksById(int Id)
        {
            string[] filters = new string[] { "Checkouts.Id" };
            string[] filters_text = new string[] { Id.ToString() };

            return _unitOfWork.Checkouts.GetWithCheckoutBooksByFilter(filters, filters_text);
        }

        public Checkout GetWithCheckoutBooksByClientId(int Id)
        {
            string[] filters = new string[] { "ClientId" };
            string[] filters_text = new string[] { Id.ToString() };
            return _unitOfWork.Checkouts.GetWithCheckoutBooksByFilter(filters, filters_text);
        }


        public Checkout CreateCheckout(Checkout newCheckout)
        {
            return _unitOfWork.Checkouts.CreateCheckout(newCheckout);
        }

        public Checkout UpdateCheckout(Checkout checkoutToBeUpdated)
        {
            return _unitOfWork.Checkouts.UpdateCheckout(checkoutToBeUpdated);
        }
    }
}
