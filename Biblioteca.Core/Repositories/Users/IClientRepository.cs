using Biblioteca.Core.Models.Users;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.Core.Repositories.Users
{
    public interface IClientRepository : IRepository<Client>
    {
        Task<Client> GetWithCheckoutByEmailAsync(string email);
    }
}
