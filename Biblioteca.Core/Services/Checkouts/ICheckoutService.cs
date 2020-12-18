using Biblioteca.Core.Models.Checkouts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.Core.Services.Checkouts
{
    public interface ICheckoutService
    {
        Task<IEnumerable<Checkout>> GetAllWithClientsAndBook();
        //Task<Checkout> GetAllWithUsersAndBook();
        //Task<Checkout> GetWithUserAndBookById(int id);
        //Task<IEnumerable<Checkout>> GetAllWithUserAndBookByUserId(int userId);


        //Task<Checkout> CreateCheckout(Checkout newCheckout);
        //Task UpdateCheckout(Checkout checkoutToBeUpdated, Checkout checkout);
    }
}
