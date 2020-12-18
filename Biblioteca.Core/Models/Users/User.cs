using System;
using System.Collections.Generic;
using System.Text;

namespace Biblioteca.Core.Models.Users
{
    public abstract class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string NIF { get; set; }
        public string Email { get; set; }
        public bool State { get; set; }
      
    }
}
