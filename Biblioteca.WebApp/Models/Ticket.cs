using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Biblioteca.WebApp.Models
{
    public class Ticket
    {
        public decimal Price { get; set; }
        public DateTime? PaymentDate { get; set; }
        public DateTime Date { get; set; }
    }
}
