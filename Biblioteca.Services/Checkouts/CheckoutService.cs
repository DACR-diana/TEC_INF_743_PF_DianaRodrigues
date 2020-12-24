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

        public async Task<IEnumerable<Checkout>> GetAllWithClientsAndBook()
        {
            return await _unitOfWork.Checkouts.GetAllWithClientsAndBookAsync();
        }

        public async Task<IEnumerable<Checkout>> GetWithUserAndBookById(int id)
        {
            return await _unitOfWork.Checkouts.GetWithUserAndBookByIdAsync(id);
        }

        public async Task<IEnumerable<Checkout>> GetAllWithUserAndBookByUserId(int userId)
        {
            return await _unitOfWork.Checkouts.GetAllWithUserAndBookByUserIdAsync(userId);
        }

        public async Task<Checkout> CreateCheckout(Checkout newCheckout)
        {
            return await _unitOfWork.Checkouts.CreateCheckout(newCheckout); 
        }

        public async Task<Checkout> UpdateCheckout(Checkout checkoutToBeUpdated)
        {
            return await _unitOfWork.Checkouts.UpdateCheckout(checkoutToBeUpdated);
        }
    }
}
