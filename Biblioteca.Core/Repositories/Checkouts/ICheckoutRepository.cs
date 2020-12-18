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
        //Task<Checkout> GetWithUserAndBookByIdAsync(int id);
        //Task<IEnumerable<Checkout>> GetAllWithUserAndBookByUserIdAsync(int userId);
    }
}
