using Biblioteca.Core;
using Biblioteca.Core.Models.Users;
using Biblioteca.Core.Services.Users;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.Services.Users
{
    public class ClientService : IClientService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ClientService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public async Task<Client> GetWithCheckoutByEmail(string email)
        {
            return await _unitOfWork.Clients.GetWithCheckoutByEmailAsync(email);
        }

        public async Task<IEnumerable<Client>> GetAllWithCheckout()
        {
            return await _unitOfWork.Clients.GetAllWithCheckoutAsync();
        }

        public async Task<Client> CreateClient(Client newClient)
        {
            await _unitOfWork.Clients.AddAsync(newClient);
            await _unitOfWork.CommitAsync();
            return newClient;
        }

        public async Task UpdateClient(Client bookToBeclient, Client client)
        {
            bookToBeclient.Name = client.Name;
            bookToBeclient.NIF = client.NIF;
            bookToBeclient.State = client.State;
            bookToBeclient.Email = client.Email;

            _unitOfWork.Clients.UpdateAsync(bookToBeclient);
            await _unitOfWork.CommitAsync();
        }
    }
}
