using Biblioteca.Core.Models.Books;
using Biblioteca.Core.Models.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace Biblioteca.Core.Models
{
    public class Checkout
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int ClientId { get; set; }
        public Client Client { get; set; }
        public DateTime DeliveryDate { get; set; }
        public DateTime ExpectedDate { get; set; }

        public ICollection<Book> Books { get; set; }
        public ICollection<Ticket> Tickets { get; set; }


        public Checkout()
        {
            this.Books = new HashSet<Book>();
        }
    }
}
