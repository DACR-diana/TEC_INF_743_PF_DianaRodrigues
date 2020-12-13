using Biblioteca.Core.Models.Users;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.Core.Services.Users
{
    public interface IEmployeeService
    {
        Task<Employee> GetByEmailAsync(string email);


        Task<Employee> CreateEmployee(Employee newEmployee);
        Task UpdateEmployee(Employee employeeToBeUpdated, Employee employee);
    }
}
