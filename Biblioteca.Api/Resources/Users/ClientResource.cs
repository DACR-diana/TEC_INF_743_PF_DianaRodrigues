using Biblioteca.Api.Resources.Checkouts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Biblioteca.Api.Resources.Users
{
    public class ClientResource : UserResource
    {
        public DateTime Registration { get; set; }
        public ICollection<CheckoutResource> Checkouts { get; set; }
    }
}
