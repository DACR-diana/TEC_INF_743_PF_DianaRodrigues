using Biblioteca.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.Core.Repositories
{
    public interface IPaymentRepository : IRepository<Payment>
    {
        //Task<IEnumerable<Payment>> GetAllPaymentsAsync();
    }
}
