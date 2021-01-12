using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Biblioteca.WebApp.Helpers;
using Biblioteca.WebApp.Models.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Biblioteca.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private string apiBaseUrl;
        private IConfiguration _Configure;
        private readonly IHttpClientHelper _clientClientHelper;

        public HomeController(IConfiguration configuration, IHttpClientHelper clientClientHelper)
        {
            _Configure = configuration;
            _clientClientHelper = clientClientHelper;
            apiBaseUrl = _Configure.GetValue<string>("WebAPIBaseUrl");
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]

        public async Task<IActionResult> Login()
        {
            var employeeNumber = Request.Query["employeeNumber"];
            var email = Request.Query["email"];

            Employee employee = new Employee();
            employee.Email = email;
            employee.EmployeeNumber = employeeNumber;

            // GET THIS WITH COOKIES LATER
            HttpContext.Session.SetString("language", "pt-PT");

            var result = await _clientClientHelper.GetContent($"{apiBaseUrl}{ HttpContext.Session.GetString("language")}/api/Employee/GetByEmail/{email}/{employeeNumber}");

            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                TempData["Employee"] = JsonConvert.SerializeObject(employeeNumber);
                return RedirectToRoute(new { Controller = "Dashboard", Action = "Index" });
            }
            else
            {
                ModelState.Clear();
                ModelState.AddModelError(string.Empty, "Username or Password is Incorrect");
                return NotFound();

            }
        }
    }
}
