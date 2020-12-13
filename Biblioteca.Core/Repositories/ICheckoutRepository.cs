using Biblioteca.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.Core.Repositories
{
    public interface ICheckoutRepository : IRepository<Checkout>
    {
        Task<IEnumerable<Checkout>> GetAllWithUsersAndBookAsync();
        Task<Checkout> GetWithUserAndBookByIdAsync(int id);
        Task<IEnumerable<Checkout>> GetAllWithUserAndBookByUserIdAsync(int userId);
    }
}
