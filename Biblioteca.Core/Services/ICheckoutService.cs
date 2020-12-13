using Biblioteca.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.Core.Services
{
    public interface ICheckoutService
    {
        Task<Checkout> GetAllWithUsersAndBook();
        Task<Checkout> GetWithUserAndBookById(int id);
        Task<IEnumerable<Checkout>> GetAllWithUserAndBookByUserId(int userId);


        Task<Checkout> CreateCheckout(Checkout newCheckout);
        Task UpdateCheckout(Checkout checkoutToBeUpdated, Checkout checkout);
    }
}
