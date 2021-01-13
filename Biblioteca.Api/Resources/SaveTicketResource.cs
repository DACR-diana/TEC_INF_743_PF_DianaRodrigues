using Biblioteca.Core.Models;
using Biblioteca.Core.Models.Checkouts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Biblioteca.Api.Resources
{
    public class SaveTicketResource
    {
        public decimal Price { get; set; }
        public bool State { get; set; }
        public int PaymentId { get; set; }
        public DateTime? PaymentDate { get; set; }
        public DateTime Date { get; set; }
        public int CheckoutId { get; set; }
    }
}
