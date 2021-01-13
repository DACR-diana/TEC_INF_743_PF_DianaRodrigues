using Biblioteca.Api.Resources.Users;
using Biblioteca.Core.Models;
using Biblioteca.Core.Models.Checkouts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Biblioteca.Api.Resources.Checkouts
{
    public class CheckoutResource
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public DateTime ExpectedDate { get; set; }
        public int ClientId { get; set; }
        public ClientResource Client { get; set; }
        public ICollection<CheckoutBookResource> CheckoutBooks { get; set; }
        public ICollection<TicketResource> Tickets { get; set; }
    }
}
