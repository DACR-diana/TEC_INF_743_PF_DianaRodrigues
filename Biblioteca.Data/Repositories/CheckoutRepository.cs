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
    public class CheckoutRepository : Repository<Checkout>, ICheckoutRepository
    {
        private ApiDbContext ApiDbContext
        {
            get { return Context as ApiDbContext; }
        }

        public CheckoutRepository(ApiDbContext context)
            : base(context)
        { }

        public async Task<IEnumerable<Checkout>> GetAllWithUsersAndBookAsync()
        {
            return await ApiDbContext.Checkouts
                .Include(m => m.Client)
                .Include(m => m.Books)
                .ToListAsync();
        }

        public async Task<Checkout> GetWithUserAndBookByIdAsync(int id)
        {
            return await ApiDbContext.Checkouts
              .Include(m => m.Client)
                .Include(m => m.Books)
                .SingleOrDefaultAsync(m => m.Id == id); ;
        }

        public async Task<IEnumerable<Checkout>> GetAllWithUserAndBookByUserIdAsync(int userId)
        {
            return await ApiDbContext.Checkouts
               .Include(m => m.Client)
                .Include(m => m.Books)
                .Where(m => m.Client.Id == userId)
                .ToListAsync();
        }
    }
}
