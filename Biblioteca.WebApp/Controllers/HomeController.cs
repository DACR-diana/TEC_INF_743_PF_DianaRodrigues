using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Biblioteca.WebApp.Models.Users;
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

        public HomeController(IConfiguration configuration)
        {
            _Configure = configuration;

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

            var httpClientHandler = new HttpClientHandler();
            httpClientHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

            using (HttpClient client = new HttpClient(httpClientHandler))
            {
                Employee employee = new Employee();
                employee.Email = email;
                employee.EmployeeNumber = employeeNumber;


                //StringContent content = new StringContent(JsonConvert.SerializeObject(employee), Encoding.UTF8, "application/json");
                string endpoint = apiBaseUrl + $"Employee/GetByEmail/{email}/{employeeNumber}";
              

                using (var Response = await client.GetAsync(endpoint))
                {
                  

                    if (Response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        TempData["Employee"] = JsonConvert.SerializeObject(employeeNumber);

                        return RedirectToRoute(new { Controller = "Dashboard", Action = "Index" });

                        //return RedirectToAction("Profile");

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
    }
}
