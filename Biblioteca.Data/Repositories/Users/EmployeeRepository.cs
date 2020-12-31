using Biblioteca.Core.Models.Users;
using Biblioteca.Core.Repositories.Users;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.Data.Repositories.Users
{
    
    public class EmployeeRepository : Repository<Employee>, IEmployeeRepository
    {
        private ApiDbContext ApiDbContext
        {
            get { return Context as ApiDbContext; }
        }

        public EmployeeRepository(ApiDbContext context)
            : base(context)
        { }

        public async Task<Employee> GetByEmailAsync(string email,string employeeNumber)
        {
            return await ApiDbContext.Employees
                .SingleOrDefaultAsync(m => m.Email == email && m.EmployeeNumber== employeeNumber);
        }
    }
}
