using Biblioteca.Core.Models;
using Biblioteca.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.Data.Repositories
{
    public class TicketRepository : Repository<Ticket>, ITicketRepository
    {
        private ApiDbContext ApiDbContext
        {
            get { return Context as ApiDbContext; }
        }

        public TicketRepository(ApiDbContext context)
            : base(context)
        { }


        public async Task<Ticket> GetWithCheckoutsByIdAsync(int id)
        {
            return await ApiDbContext.Tickets
                .SingleOrDefaultAsync(m => m.Id == id); 
        }

        public async Task<Ticket> GetAllWithCheckoutsByCheckoutsIdAsync(int checkoutId)
        {
            return await ApiDbContext.Tickets
                .SingleOrDefaultAsync(m => m.CheckoutId == checkoutId);
        }
    }
}
