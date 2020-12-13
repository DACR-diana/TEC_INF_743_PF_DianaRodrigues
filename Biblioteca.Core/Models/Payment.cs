using System;
using System.Collections.Generic;
using System.Text;

namespace Biblioteca.Core.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Ticket> Tickets { get; set; }
    }
}
