using Biblioteca.WebApp.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Biblioteca.WebApp.Models.Checkouts
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
    }
}
