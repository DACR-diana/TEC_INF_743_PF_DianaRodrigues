using Biblioteca.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.Core.Services
{
    public interface ITicketService
    {
        Task<IEnumerable<Ticket>> GetAllTickets();
        Task<Ticket> GetAllWithCheckoutsByCheckoutsId(int checkoutId);
        Task<Ticket> GetAllWithCheckoutsByCheckoutsIdAndState(int checkoutId, bool state);


        Task<Ticket> CreateTicket(Ticket newTicket);
        Task UpdateTicket(Ticket ticketToUpdate);
    }
}
