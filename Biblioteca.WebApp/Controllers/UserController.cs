using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Biblioteca.WebApp.Models.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Biblioteca.WebApp.Controllers
{
    public class UserController : Controller
    {
        private string apiBaseUrl;
        private IConfiguration _Configure;

        public UserController(IConfiguration configuration)
        {
            _Configure = configuration;

            apiBaseUrl = _Configure.GetValue<string>("WebAPIBaseUrl");
        }

        #region GET'S

        private async Task<JsonResult> GetUser(string email)
        {
            var httpClientHandler = new HttpClientHandler();
            httpClientHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            Client client = new Client();

            using (HttpClient httpClient = new HttpClient(httpClientHandler))
            {
                string endpoint = $"{apiBaseUrl}Client/GetWithCheckoutByEmail/{email}";

                using (var Response = await httpClient.GetAsync(endpoint))
                {
                    var result = await Response.Content.ReadAsStringAsync();
                    client = JsonConvert.DeserializeObject<Client>(result);
                }

            }
            return new JsonResult(client);
        }

        private async Task<JsonResult> GetClients()
        {
            var httpClientHandler = new HttpClientHandler();
            httpClientHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            var clients = new List<Client>();

            using (HttpClient client = new HttpClient(httpClientHandler))
            {
                string endpoint = $"{apiBaseUrl}Client/GetAllWithCheckout";
                using (var Response = await client.GetAsync(endpoint))
                {
                    var result = await Response.Content.ReadAsStringAsync();
                    clients = JsonConvert.DeserializeObject<List<Client>>(result);
                }

            }
            return new JsonResult(clients);
        }

        #endregion

        #region VIEWS

        public IActionResult Add()
        {
            return View();
        }

        public IActionResult GoToCheckoutAddView()
        {
            return RedirectToRoute(new { Controller = "Checkout", Action = "Add" });
        }
        public async Task<IActionResult> Update(string email)
        {
            ViewBag.User = await GetUser(email);
            HttpContext.Session.SetString("clientEmail", email);

            return View();
        }

        public async Task<IActionResult> List()
        {
            ViewBag.Users = await GetClients();
            return View("List");
        }

        #endregion

        public IActionResult ErrorMessage()
        {
            return Content("Ocorreu algum erro");
        }

        #region Actions

        public async Task<IActionResult> AddClient()
        {
            var name = Request.Form["name"];
            var NIF = Request.Form["NIF"];
            var email = Request.Form["email"];

            if (name.Count == 0 || NIF.Count == 0 || email.Count == 0)
                return ErrorMessage();
            else
            {
                var httpClientHandler = new HttpClientHandler();
                httpClientHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

                using (HttpClient httpClient = new HttpClient(httpClientHandler))
                {

                    Client client = new Client();
                    client.Email = email;
                    client.NIF = NIF;
                    client.Name = name;
                    client.State = true;
                    client.Registration = DateTime.Now;


                    HttpContent content = new StringContent(JsonConvert.SerializeObject(client), Encoding.UTF8, "application/json");
                    string endpoint = $"{apiBaseUrl}Client/CreateClient";
                    using (var Response = await httpClient.PostAsync(endpoint, content))
                    {
                        if (Response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            return GoToCheckoutAddView();
                        }
                        else
                        {
                            ModelState.Clear();
                            return ErrorMessage();
                        }

                    }
                }
            }
        }

        public async Task<IActionResult> SaveClient()
        {
            var name = Request.Form["name"];
            var NIF = Request.Form["NIF"];
            var state = Request.Form["customSwitch3"];

            if (Request.Form["customSwitch3"].Count > 1)
                state = Request.Form["customSwitch3"][0];

            if (name.Count == 0 || NIF.Count == 0)
                return ErrorMessage();
            else
            {
                var httpClientHandler = new HttpClientHandler();
                httpClientHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

                using (HttpClient httpClient = new HttpClient(httpClientHandler))
                {

                    Client client = new Client();
                    client.Email = HttpContext.Session.GetString("clientEmail");
                    client.NIF = NIF;
                    client.Name = name;
                    client.State = bool.Parse(state);

                    HttpContent content = new StringContent(JsonConvert.SerializeObject(client), Encoding.UTF8, "application/json");
                    string endpoint = apiBaseUrl + $"Client/UpdateClient";
                    using (var Response = await httpClient.PostAsync(endpoint, content))
                    {
                        if (Response.StatusCode == System.Net.HttpStatusCode.OK)
                            return await List();
                        else
                        {
                            ModelState.Clear();
                            return ErrorMessage();
                        }

                    }
                }
            }
        }
        #endregion
    }
}
