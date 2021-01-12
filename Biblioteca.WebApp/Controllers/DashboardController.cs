using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Biblioteca.WebApp.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

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

        public IActionResult Index()
        {

            //if (TempData["language"] == null)
            //    TempData["language"] = "pt-PT";

            //var content = await _localization.GetContent(TempData["language"].ToString(), "Book", apiBaseUrl);
            return View();
        }

        public IActionResult ErrorMessage()
        {
            return Content("Ocorreu algum erro");
        }
    }
}
