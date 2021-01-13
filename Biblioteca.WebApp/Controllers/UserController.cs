using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Biblioteca.WebApp.Helpers;
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
        private readonly IHttpClientHelper _clientClientHelper;

        public UserController(IConfiguration configuration, IHttpClientHelper clientClientHelper)
        {
            _Configure = configuration;
            _clientClientHelper = clientClientHelper;
            apiBaseUrl = _Configure.GetValue<string>("WebAPIBaseUrl");
        }

        #region GET'S

        private async Task<JsonResult> GetUser(string email)
        {

            var result = await _clientClientHelper.GetContent($"{apiBaseUrl}{ HttpContext.Session.GetString("language")}/api/Client/GetWithCheckoutByEmail/{email}");
            var resutlJson = await result.Content.ReadAsStringAsync();

            Client client = JsonConvert.DeserializeObject<Client>(resutlJson);

            HttpContext.Session.SetString("clientEmail", email);
            HttpContext.Session.SetString("clientId", client.Id.ToString());
            HttpContext.Session.SetString("clientName", client.Name);


            return new JsonResult(client);
        }

        public async Task<string> CheckClient(string email)
        {
            var result = await _clientClientHelper.GetContent($"{apiBaseUrl}{ HttpContext.Session.GetString("language")}/api/Client/GetWithCheckoutByEmail/{email}");
            var resutlJson = await result.Content.ReadAsStringAsync();
            return resutlJson;
        }

        private async Task<JsonResult> GetClients()
        {
            var httpClientHandler = new HttpClientHandler();
            httpClientHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            var clients = new List<Client>();

            using (HttpClient client = new HttpClient(httpClientHandler))
            {
                string endpoint = $"{apiBaseUrl}{ HttpContext.Session.GetString("language")}/api/Client/GetAllWithCheckout";
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

        public IActionResult GoToCheckoutUserView()
        {
            return RedirectToRoute(new { Controller = "Checkout", Action = "CheckoutUser" });
        }

        public async Task<IActionResult> Update(string email)
        {

            var result = await GetUser(email);

            var json = JsonConvert.SerializeObject(result.Value);
            var client = JsonConvert.DeserializeObject<Client>(json);

            ViewBag.User = result;



            HttpContext.Session.SetString("clientId", client.Id.ToString());
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
                    string teste = JsonConvert.SerializeObject(client);
                    string endpoint = $"{apiBaseUrl}{ HttpContext.Session.GetString("language")}/api/Client/CreateClient";
                    using (var Response = await httpClient.PostAsync(endpoint, content))
                    {
                        if (Response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            var result = await Response.Content.ReadAsStringAsync();
                            var clientDatabase = JsonConvert.DeserializeObject<Client>(result);

                            HttpContext.Session.SetString("clientName", name);
                            HttpContext.Session.SetString("clientId", clientDatabase.Id.ToString());
                            return await List();
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
                    string endpoint = apiBaseUrl + $"{ HttpContext.Session.GetString("language")}/api/Client/UpdateClient";
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
