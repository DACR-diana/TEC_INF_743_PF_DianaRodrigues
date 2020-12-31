using Biblioteca.Core;
using Biblioteca.Core.Models;
using Biblioteca.Core.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;
        public PaymentService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<Payment>> GetAllPayments()
        {
            return await _unitOfWork.Payments.GetAllAsync();
        }
    }
}
