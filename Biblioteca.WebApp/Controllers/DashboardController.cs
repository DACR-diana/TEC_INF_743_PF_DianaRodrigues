using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Biblioteca.WebApp.Helpers;
using Biblioteca.WebApp.Models.Checkouts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Biblioteca.WebApp.Controllers
{
    public class DashboardController : Controller
    {


        private string apiBaseUrl;
        private IConfiguration _Configure;
        private readonly IHttpClientHelper _clientClientHelper;

        public DashboardController(IConfiguration configuration, IHttpClientHelper clientClientHelper)
        {
            _Configure = configuration;
            _clientClientHelper = clientClientHelper;

            // API URL
            apiBaseUrl = _Configure.GetValue<string>("WebAPIBaseUrl");

        }

        public async Task<IActionResult> Index()
        {
            // WHENEVER THE DASHBOARD IS CALLED, CHECK IF ANY CHECKOUT IS OVERDUE
            var checkouts = await GetExpiredCheckoutsAndApplyTicket();

            ViewBag.Info= await GetClientsCount();
            return View(checkouts);
        }

        // FUNCTION TO SHOW ERROR MESSAGE
        public IActionResult ErrorMessage()
        {
            return Content("Ocorreu algum erro");
        }


        private async Task<JsonResult> GetExpiredCheckoutsAndApplyTicket()
        {
            //API CALL
            var result = await _clientClientHelper.GetContent($"{apiBaseUrl}{ HttpContext.Session.GetString("language")}/api/Checkout/GetExpiredCheckoutsAndApplyTicket");
            var resultJson = await result.Content.ReadAsStringAsync();

            var checkouts = JsonConvert.DeserializeObject<List<Checkout>>(resultJson);
            ViewBag.TicketsCount = checkouts.Count;
            return new JsonResult(checkouts);
        }

        private async Task<List<int>> GetClientsCount()
        {
            // API CALL
            var result = await _clientClientHelper.GetContent($"{apiBaseUrl}{ HttpContext.Session.GetString("language")}/api/Checkout/GetDashboardInformation");
            var resultJson = await result.Content.ReadAsStringAsync();

            // SPLIT THE COUNTS
            var info = JsonConvert.DeserializeObject<List<int>>(resultJson);

            return info;
        }
    }
}
