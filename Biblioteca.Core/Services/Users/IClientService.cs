using Biblioteca.Core.Models.Users;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.Core.Services.Users
{
    public interface IClientService
    {
        Task<Client> GetWithCheckoutByEmail(string email);
        Task<IEnumerable<Client>> GetAllWithCheckout();

        Task<Client> CreateClient(Client newClient);
        Task UpdateClient(Client clientToBeUpdated, Client client);
    }
}
