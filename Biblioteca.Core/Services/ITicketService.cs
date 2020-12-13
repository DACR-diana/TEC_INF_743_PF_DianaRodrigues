using Biblioteca.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.Core.Services
{
    public interface ITicketService
    {
        Task<Ticket> GetWithCheckoutsByIdAsync(int id);
        Task<IEnumerable<Ticket>> GetAllWithCheckoutsByCheckoutsIdAsync(int checkoutId);


        Task<Ticket> CreateTicket(Ticket newTicket);
        Task UpdateTicket(Ticket ticketToBeUpdated, Ticket ticket);
        //Task DeleteBook(Book book);
    }
}
