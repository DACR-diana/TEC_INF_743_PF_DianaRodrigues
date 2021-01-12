using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Biblioteca.WebApp.Helpers;
using Biblioteca.WebApp.Models;
using Biblioteca.WebApp.Models.Books;
using Biblioteca.WebApp.Models.Checkouts;
using Biblioteca.WebApp.Models.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Biblioteca.WebApp.Controllers
{
    public class CheckoutController : Controller
    {
        private string apiBaseUrl;
        private IConfiguration _Configure;
        private readonly IHttpClientHelper _clientClientHelper;
        public CheckoutController(IConfiguration configuration, IHttpClientHelper clientClientHelper)
        {
            _Configure = configuration;
            _clientClientHelper = clientClientHelper;

            apiBaseUrl = _Configure.GetValue<string>("WebAPIBaseUrl");
        }

        public IActionResult ErrorMessage()
        {
            return Content("Ocorreu algum erro na API ");
        }

        #region VIEWS

        public async Task<IActionResult> Add()
        {
            ViewBag.ClientName = HttpContext.Session.GetString("clientName");
            ViewBag.ClientEmail = HttpContext.Session.GetString("clientEmail");
            return View(await GetBooks());
        }


        public async Task<IActionResult> Update()
        {
            ViewBag.ClientName = HttpContext.Session.GetString("clientName");
            ViewBag.ClientEmail = HttpContext.Session.GetString("clientEmail");
            ViewBag.Books = await GetCheckoutsWithUserByClientId(int.Parse(HttpContext.Session.GetString("clientId")));
            return View();
        }

        //public async Task<IActionResult> CheckoutUser()
        //{
        //    var checkout = await GetCheckoutsWithUserByClientId(int.Parse(HttpContext.Session.GetString("clientId")));
        //    ViewBag.ClientName = HttpContext.Session.GetString("clientName");
        //    ViewBag.ClientEmail = HttpContext.Session.GetString("clientEmail");
        //    return View("CheckoutUser", checkout);
        //}

        public async Task<IActionResult> CheckoutUser(int clientId)
        {
            var checkout = await GetWithCheckoutBooksById(clientId);
            ViewBag.ClientName = HttpContext.Session.GetString("clientName");
            ViewBag.ClientEmail = HttpContext.Session.GetString("clientEmail");
            return View("CheckoutUser", checkout);
        }


        public async Task<IActionResult> CheckoutUserCheckouts(int clientId)
        {
            var checkout = await GetCheckoutsWithUserByClientId(clientId);
            ViewBag.ClientName = HttpContext.Session.GetString("clientName");
            ViewBag.ClientEmail = HttpContext.Session.GetString("clientEmail");
            return View("CheckoutUserCheckouts", checkout);
        }

        public async Task<IActionResult> CheckoutListUser()
        {
            ViewBag.Users = await GetClients();
            return View("CheckoutListUser");
        }

        #endregion

        #region GET

        private async Task<JsonResult> GetClients()
        {

            var result = await _clientClientHelper.GetContent($"{apiBaseUrl}{ HttpContext.Session.GetString("language")}/api/Client/GetAllWithCheckout");
            var jsonResult = await result.Content.ReadAsStringAsync();
            var clients = JsonConvert.DeserializeObject<List<Client>>(jsonResult);
            return new JsonResult(clients);
        }

        private async Task<JsonResult> GetBooks()
        {
            var httpClientHandler = new HttpClientHandler();
            httpClientHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            var books = new List<Book>();

            using (HttpClient client = new HttpClient(httpClientHandler))
            {
                string endpoint = $"{apiBaseUrl}{ HttpContext.Session.GetString("language")}/api/Book/GetAllByState/{true}";
                using (var Response = await client.GetAsync(endpoint))
                {
                    var result = await Response.Content.ReadAsStringAsync();
                    books = JsonConvert.DeserializeObject<List<Book>>(result);
                }

            }
            return new JsonResult(books);
        }

        private async Task<JsonResult> GetWithCheckoutBooksById(int id)
        {

            var result = await _clientClientHelper.GetContent($"{apiBaseUrl}{ HttpContext.Session.GetString("language")}/api/Checkout/GetWithCheckoutBooksById/{id}");
            var resultJson = await result.Content.ReadAsStringAsync();
            var checkouts = JsonConvert.DeserializeObject<List<Checkout>>(resultJson);
            return new JsonResult(checkouts);
        }

        private async Task<JsonResult> GetCheckoutsWithUserByClientId(int clientId)
        {
            var httpClientHandler = new HttpClientHandler();
            httpClientHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            var checkouts = new List<Checkout>();

            using (HttpClient client = new HttpClient(httpClientHandler))
            {
                string endpoint = $"{apiBaseUrl}{ HttpContext.Session.GetString("language")}/api/Checkout/GetWithCheckoutBooksByClientId/{clientId}";
                using (var Response = await client.GetAsync(endpoint))
                {
                    var result = await Response.Content.ReadAsStringAsync();
                    checkouts = JsonConvert.DeserializeObject<List<Checkout>>(result);
                }

            }
            return new JsonResult(checkouts);
        }

        public async Task<JsonResult> GetCheckoutsWithUserByClientIdByState(int clientId, bool state)
        {
            var httpClientHandler = new HttpClientHandler();
            httpClientHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            var checkouts = new List<Checkout>();

            using (HttpClient client = new HttpClient(httpClientHandler))
            {
                string endpoint = $"{apiBaseUrl}{ HttpContext.Session.GetString("language")}/api/Checkout/GetWithCheckoutBooksByClientIdAndState/{clientId}/{state}";
                using (var Response = await client.GetAsync(endpoint))
                {
                    var result = await Response.Content.ReadAsStringAsync();
                    checkouts = JsonConvert.DeserializeObject<List<Checkout>>(result);
                }

            }
            return new JsonResult(checkouts);
        }

        #endregion


        #region Actions

        public async Task<IActionResult> CreateCheckout()
        {
            var books = Request.Form["books"];

            if (books.Count == 0)
                return ErrorMessage();
            else
            {
                var httpClientHandler = new HttpClientHandler();
                httpClientHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

                using (HttpClient client = new HttpClient(httpClientHandler))
                {

                    Checkout checkout = new Checkout();
                    checkout.Date = DateTime.Now;
                    checkout.ClientId = int.Parse(HttpContext.Session.GetString("clientId"));
                    checkout.ExpectedDate = DateTime.Now.AddDays(7);

                    List<CheckoutBook> checkoutBooks = new List<CheckoutBook>();

                    foreach (var book in books)
                    {
                        var bookForCheckout = new Book() { Id = int.Parse(book) };
                        checkoutBooks.Add(new CheckoutBook() { Book = bookForCheckout });
                    }


                    checkout.CheckoutBooks = checkoutBooks;

                    HttpContent content = new StringContent(JsonConvert.SerializeObject(checkout), Encoding.UTF8, "application/json");
                    string endpoint = apiBaseUrl + $"{ HttpContext.Session.GetString("language")}/api/Checkout/CreateCheckout";
                    using (var Response = await client.PostAsync(endpoint, content))
                    {
                        if (Response.StatusCode == System.Net.HttpStatusCode.OK)
                            return await CheckoutUser(checkout.ClientId);
                        else
                        {
                            ModelState.Clear();
                            return ErrorMessage();
                        }

                    }

                }
            }
        }

        public async Task<IActionResult> SaveCheckout()
        {
            var isbn = Request.Form["isbn"];
            var title = Request.Form["title"];
            var categories = Request.Form["categories"];
            var authors = Request.Form["authors"];
            var country = Request.Form["country"];
            var state = Request.Form["customSwitch3"];

            if (Request.Form["customSwitch3"].Count > 1)
                state = Request.Form["customSwitch3"][0];

            if (categories.Count == 0 || authors.Count == 0 || country.Count == 0)
                return ErrorMessage();
            else
            {
                var httpClientHandler = new HttpClientHandler();
                httpClientHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

                using (HttpClient client = new HttpClient(httpClientHandler))
                {

                    Book book = new Book();
                    book.Id = int.Parse(HttpContext.Session.GetString("bookId"));
                    book.CountryId = int.Parse(country);
                    book.ISBN = int.Parse(isbn);
                    book.Title = title;
                    book.State = bool.Parse(state);

                    List<Author> authorsList = new List<Author>();
                    List<Category> categoriesList = new List<Category>();

                    foreach (var author in authors)
                        authorsList.Add(new Author() { Id = int.Parse(author) });

                    foreach (var category in categories)
                        categoriesList.Add(new Category() { Id = int.Parse(category) });


                    book.Authors = authorsList;
                    book.Categories = categoriesList;

                    HttpContent content = new StringContent(JsonConvert.SerializeObject(book), Encoding.UTF8, "application/json");
                    string teste = JsonConvert.SerializeObject(book);
                    string endpoint = apiBaseUrl + $"{ HttpContext.Session.GetString("language")}/api/Book/UpdateBook";
                    using (var Response = await client.PostAsync(endpoint, content))
                    {
                        if (Response.StatusCode == System.Net.HttpStatusCode.OK)
                            return await CheckoutUser(1);
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
