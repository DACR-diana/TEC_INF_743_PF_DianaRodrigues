using System;
using System.Collections.Generic;
using System.Text;

namespace Biblioteca.Core.Models.Users
{
    public class Client : User
    {
        public DateTime Registration { get; set; }

        public ICollection<Checkout> Checkouts { get; set; }


        public Client() 
        {
            this.Checkouts = new HashSet<Checkout>();
        }
    }
}
