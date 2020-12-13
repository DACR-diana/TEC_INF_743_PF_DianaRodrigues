using Biblioteca.Core.Models.Users;
using Biblioteca.Core.Repositories.Users;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.Data.Repositories.Users
{
    public class ClientRepository : Repository<Client>, IClientRepository
    {
        private ApiDbContext ApiDbContext
        {
            get { return Context as ApiDbContext; }
        }

        public ClientRepository(ApiDbContext context)
            : base(context)
        { }

        public async Task<Client> GetWithCheckoutByEmailAsync(string email)
        {
            return await ApiDbContext.Clients
                .Include(m => m.Checkouts)
                .SingleOrDefaultAsync(m => m.Email == email);
        }
    }
}
