using Biblioteca.Core.Models.Users;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.Core.Services.Users
{
    public interface IEmployeeService
    {
        Task<Employee> GetByEmail(string email, string employeeNumber);
    }
}
