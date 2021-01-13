using Biblioteca.Core;
using Biblioteca.Core.Models;
using Biblioteca.Core.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.Services
{
    public class TicketService : ITicketService
    {
        private readonly IUnitOfWork _unitOfWork;
        public TicketService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Ticket>> GetAllTickets()
        {
            return await _unitOfWork.Tickets.GetAllAsync();
        }

        public async Task<Ticket> GetAllWithCheckoutsByCheckoutsId(int checkoutId)
        {
            return await _unitOfWork.Tickets.GetAllWithCheckoutsByCheckoutsIdAsync(checkoutId);
        }

        public async Task<Ticket> CreateTicket(Ticket newTicket)
        {
            await _unitOfWork.Tickets.AddAsync(newTicket);
            await _unitOfWork.CommitAsync();
            return newTicket;
        }


        public async Task UpdateTicket(Ticket ticketToBeUpdated, Ticket ticket)
        {
            ticketToBeUpdated.PaymentDate = ticket.PaymentDate;
            ticketToBeUpdated.State = ticket.State;

            _unitOfWork.Tickets.UpdateAsync(ticketToBeUpdated);
            await _unitOfWork.CommitAsync();
        }
    }
}
