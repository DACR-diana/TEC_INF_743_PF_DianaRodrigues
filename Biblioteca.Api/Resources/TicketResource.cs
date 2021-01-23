using Biblioteca.Api.Resources.Checkouts;
using Biblioteca.Core.Models;
using Biblioteca.Core.Models.Checkouts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Biblioteca.Api.Resources
{
    public class TicketResource
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
        public PaymentResource Payment { get; set; }
        public DateTime? PaymentDate { get; set; }
        public DateTime Date { get; set; }
        public CheckoutResource Checkout { get; set; }
    }
}
