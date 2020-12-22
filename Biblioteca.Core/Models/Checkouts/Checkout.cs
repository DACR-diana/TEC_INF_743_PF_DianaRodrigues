using Biblioteca.Core.Models.Books;
using Biblioteca.Core.Models.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace Biblioteca.Core.Models.Checkouts
{
    public class Checkout
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public DateTime ExpectedDate { get; set; }
        public int ClientId { get; set; }
        public Client Client { get; set; }

        public ICollection<CheckoutBook> CheckoutBooks { get; set; }
        public ICollection<Ticket> Tickets { get; set; }

    }
}
