using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Biblioteca.Api.Resources.Users
{
    public class UserResource
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string NIF { get; set; }
        public string Email { get; set; }
        public bool State { get; set; }
    }
}
