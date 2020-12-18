using Biblioteca.Core.Models.Checkouts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Biblioteca.Core.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
        public bool State { get; set; }
        public int PaymentId { get; set; }
        public Payment Payment { get; set; }
        public DateTime PaymentDate { get; set; }
        public DateTime Date { get; set; }
        public int CheckoutId { get; set; }
        public Checkout Checkout { get; set; }
    }
}
