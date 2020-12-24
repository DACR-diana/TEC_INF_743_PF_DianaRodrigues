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
        Task<IEnumerable<Checkout>> GetAllWithClientsAndBookAsync();
        Task<IEnumerable<Checkout>> GetWithUserAndBookByIdAsync(int id);
        Task<IEnumerable<Checkout>> GetAllWithUserAndBookByUserIdAsync(int userId);


        Task<Checkout> CreateCheckout(Checkout newCheckout);
        Task<Checkout> UpdateCheckout(Checkout checkoutToBeUpdated);
    }
}
