using Biblioteca.Core;
using Biblioteca.Core.Models.Users;
using Biblioteca.Core.Services.Users;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.Services.Users
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IUnitOfWork _unitOfWork;
        public EmployeeService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public async Task<Employee> GetByEmail(string email, string employeeNumber)
        {
            return await _unitOfWork.Employees.GetByEmailAsync(email, employeeNumber);
        }
    }
}
