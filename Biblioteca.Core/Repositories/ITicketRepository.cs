using Biblioteca.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.Core.Repositories
{
    public interface ITicketRepository : IRepository<Ticket>
    {
        Task<Ticket> GetWithCheckoutsByIdAsync(int id);
        Task<Ticket> GetAllWithCheckoutsByCheckoutsIdAsync(int checkoutId);
    }
}
