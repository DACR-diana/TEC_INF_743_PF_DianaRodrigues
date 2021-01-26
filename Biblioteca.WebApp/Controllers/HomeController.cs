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
        // Dependency Injection
        private string apiBaseUrl;
        private IConfiguration _Configure;
        private readonly IHttpClientHelper _clientClientHelper;

        public HomeController(IConfiguration configuration, IHttpClientHelper clientClientHelper)
        {
            _Configure = configuration;
            _clientClientHelper = clientClientHelper;
            apiBaseUrl = _Configure.GetValue<string>("WebAPIBaseUrl"); // GET'S THE API URL FROM FILE appsettings.json
        }

        public IActionResult Index()
        {
            return View();
        }


        // FUNCTION TO HANDLE LOGIN
        public async Task<IActionResult> Login(string email,string num)
        {
            Employee employee = new Employee();
            employee.Email = email;
            employee.EmployeeNumber = num;

            // GET THIS WITH COOKIES LATER
            HttpContext.Session.SetString("language", "pt-PT");

            var result = await _clientClientHelper.GetContent($"{apiBaseUrl}{ HttpContext.Session.GetString("language")}/api/Employee/GetByEmail/{email}/{num}");

            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                // SAVE EMPLOYEE NUMBER AND RETURN SUCESS CODE
                TempData["Employee"] = JsonConvert.SerializeObject(num);
                return Json(new { Status = "200" });
            }
            else
            {
                // RETURN ERROR CODE
                ModelState.Clear();
                return Json(new { Status = "403" });
            }
        }

        // GO TO DASHBOARD FUNCTION
        public IActionResult GoToDashboard()
        {
            return RedirectToRoute(new { Controller = "Dashboard", Action = "Index" });
        }
    }
}
