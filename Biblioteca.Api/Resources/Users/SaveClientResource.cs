using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Biblioteca.Api.Resources.Users
{
    public class SaveClientResource:UserResource
    {
        public DateTime Registration { get; set; }
    }
}
