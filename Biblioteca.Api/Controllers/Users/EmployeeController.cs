using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Biblioteca.Core.Models.Users;
using Biblioteca.Core.Services.Users;
using AutoMapper;
using Biblioteca.Api.Resources.Users;

namespace Biblioteca.Api.Controllers.Users
{
    [Route("{culture:culture}/api/[controller]")]
    [ApiController]
    public class EmployeeController : Controller
    {
        // Dependency Injection
        private readonly IEmployeeService _employeeService;
        private readonly IMapper _mapper;

        public EmployeeController(IEmployeeService employeeService, IMapper mapper)
        {
            this._mapper = mapper;
            this._employeeService = employeeService;
        }

        [HttpGet("GetByEmail/{email}/{employeeNumber}")]
        public async Task<ActionResult<EmployeeResource>> GetByEmail(string email,string employeeNumber)
        {
            var employee = await _employeeService.GetByEmail(email, employeeNumber);

            if (employee == null)
                return NotFound();
            else
            {
                var employeeResource = _mapper.Map<Employee, EmployeeResource>(employee);
                return Ok(employeeResource);
            }
           
        }
    }
}
