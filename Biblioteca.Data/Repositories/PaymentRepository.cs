using Biblioteca.Core.Models;
using Biblioteca.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Biblioteca.Data.Repositories
{

    public class PaymentRepository : Repository<Payment>, IPaymentRepository
    {
        private ApiDbContext ApiDbContext
        {
            get { return Context as ApiDbContext; }
        }

        public PaymentRepository(ApiDbContext context)
            : base(context)
        { }
    }
}
