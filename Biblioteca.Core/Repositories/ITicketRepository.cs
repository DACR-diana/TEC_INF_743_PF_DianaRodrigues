using Biblioteca.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.Core.Repositories
{
    public interface ITicketRepository : IRepository<Ticket>
    {
        //Task<IEnumerable<Ticket>> GetAllWithCheckoutsAsync();
        Task<Ticket> GetWithCheckoutsByIdAsync(int id);
        Task<IEnumerable<Ticket>> GetAllWithCheckoutsByCheckoutsIdAsync(int checkoutId);
    }
}
