using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Biblioteca.Api.Resources.Users
{
    public class EmployeeResource : UserResource
    {
        public string EmployeeNumber { get; set; }
    }
}
