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
        //private ILocalizationHelper _localization;

        public DashboardController(IConfiguration configuration, IHttpClientHelper clientClientHelper)
        {
            _Configure = configuration;
            _clientClientHelper = clientClientHelper;

            apiBaseUrl = _Configure.GetValue<string>("WebAPIBaseUrl");

        }

        public async Task<IActionResult> Index()
        {

            //if (TempData["language"] == null)
            //    TempData["language"] = "pt-PT";

            //var content = await _localization.GetContent(TempData["language"].ToString(), "Book", apiBaseUrl);
            var checkouts = await GetExpiredCheckoutsAndApplyTicket();

            ViewBag.ClientsCount= await GetClientsCount();
            return View(checkouts);
        }

        public IActionResult ErrorMessage()
        {
            return Content("Ocorreu algum erro");
        }


        private async Task<JsonResult> GetExpiredCheckoutsAndApplyTicket()
        {

            var result = await _clientClientHelper.GetContent($"{apiBaseUrl}{ HttpContext.Session.GetString("language")}/api/Checkout/GetExpiredCheckoutsAndApplyTicket");
            var resultJson = await result.Content.ReadAsStringAsync();

            var checkouts = JsonConvert.DeserializeObject<List<Checkout>>(resultJson);
            ViewBag.TicketsCount = checkouts.Count;
            return new JsonResult(checkouts);
        }

        private async Task<int> GetClientsCount()
        {

            var result = await _clientClientHelper.GetContent($"{apiBaseUrl}{ HttpContext.Session.GetString("language")}/api/Checkout/GetCountClient");
            var resultJson = await result.Content.ReadAsStringAsync();
            return int.Parse(resultJson);
        }
    }
}
