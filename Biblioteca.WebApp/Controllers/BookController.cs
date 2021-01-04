using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Biblioteca.WebApp.Models;
using Biblioteca.WebApp.Models.Books;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Biblioteca.WebApp.Controllers
{
    public class BookController : Controller
    {
        private string apiBaseUrl;
        private IConfiguration _Configure;

        public BookController(IConfiguration configuration)
        {
            _Configure = configuration;

            apiBaseUrl = _Configure.GetValue<string>("WebAPIBaseUrl");
        }


        public IActionResult ErrorMessage()
        {
            return Content("Ocorreu algum erro");
        }

        #region GET

        private async Task<List<Country>> GetCountries()
        {
            var httpClientHandler = new HttpClientHandler();
            httpClientHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

            List<Country> countries = new List<Country>();

            using (HttpClient client = new HttpClient(httpClientHandler))
            {

                string endpoint = apiBaseUrl + $"Country/GetAllCountries";
                using (var Response = await client.GetAsync(endpoint))
                {
                    var result = await Response.Content.ReadAsStringAsync();
                    countries = JsonConvert.DeserializeObject<List<Country>>(result);
                }
            }
            return countries;
        }

        private async Task<List<Author>> GetAuthors()
        {
            var httpClientHandler = new HttpClientHandler();
            httpClientHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

            List<Author> authors = new List<Author>();

            using (HttpClient client = new HttpClient(httpClientHandler))
            {

                string endpoint = apiBaseUrl + $"Author/GetAllAuthors";
                using (var Response = await client.GetAsync(endpoint))
                {
                    var result = await Response.Content.ReadAsStringAsync();
                    authors = JsonConvert.DeserializeObject<List<Author>>(result);
                }
            }
            return authors;
        }

        private async Task<List<Category>> GetCategories()
        {
            var httpClientHandler = new HttpClientHandler();
            httpClientHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

            List<Category> categories = new List<Category>();

            using (HttpClient client = new HttpClient(httpClientHandler))
            {

                string endpoint = $"{apiBaseUrl}Category/GetAllCategories";
                using (var Response = await client.GetAsync(endpoint))
                {
                    var result = await Response.Content.ReadAsStringAsync();
                    categories = JsonConvert.DeserializeObject<List<Category>>(result);
                }
            }
            return categories;
        }

        private async Task<JsonResult> GetBooks()
        {
            var httpClientHandler = new HttpClientHandler();
            httpClientHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            var books = new List<Book>();

            using (HttpClient client = new HttpClient(httpClientHandler))
            {
                string endpoint = apiBaseUrl + $"Book/GetAllWithCategoriesAndAuthor";
                using (var Response = await client.GetAsync(endpoint))
                {
                    var result = await Response.Content.ReadAsStringAsync();
                    books = JsonConvert.DeserializeObject<List<Book>>(result);
                }

            }
            return new JsonResult(books);
        }

        private async Task<JsonResult> GetBook(int id)
        {
            var httpClientHandler = new HttpClientHandler();
            httpClientHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            Book book = new Book();

            using (HttpClient client = new HttpClient(httpClientHandler))
            {
                string endpoint = $"{apiBaseUrl}Book/GetWithCategoriesAndAuthorById/{id}";

                using (var Response = await client.GetAsync(endpoint))
                {
                    var result = await Response.Content.ReadAsStringAsync();
                    book = JsonConvert.DeserializeObject<Book>(result);
                }

            }
            return new JsonResult(book);
        }

        #endregion

        #region Views
        public async Task<IActionResult> Update(int id)
        {
            ViewBag.Book = await GetBook(id);
            HttpContext.Session.SetString("bookId",id.ToString());
            AddViewModel categoriesAuthors = new AddViewModel()
            {
                categories = await GetCategories(),
                authors = await GetAuthors(),
                countries = await GetCountries()
            };
            return View(categoriesAuthors);
        }
        public async Task<IActionResult> List()
        {
            ViewBag.Books = await GetBooks();
            return View("List");
        }

        public async Task<IActionResult> Add()
        {
            AddViewModel categoriesAuthors = new AddViewModel()
            {
                categories = await GetCategories(),
                authors = await GetAuthors(),
                countries = await GetCountries()
            };
            return View(categoriesAuthors);
        }

        public IActionResult Index()
        {
            return View();
        }
        #endregion

        #region Actions

        public async Task<IActionResult> AddBook()
        {
            var isbn = Request.Form["isbn"];
            var title = Request.Form["title"];
            var categories = Request.Form["categories"];
            var authors = Request.Form["authors"];
            var country = Request.Form["country"];

            if (categories.Count == 0 || authors.Count == 0 || country.Count == 0)
                return ErrorMessage();
            else
            {
                var httpClientHandler = new HttpClientHandler();
                httpClientHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

                using (HttpClient client = new HttpClient(httpClientHandler))
                {

                    Book book = new Book();
                    book.CountryId = int.Parse(country);
                    book.ISBN = int.Parse(isbn);
                    book.Title = title;
                    book.State = true;

                    List<Author> authorsList = new List<Author>();
                    List<Category> categoriesList = new List<Category>();

                    foreach (var author in authors)
                        authorsList.Add(new Author() { Id = int.Parse(author) });

                    foreach (var category in categories)
                        categoriesList.Add(new Category() { Id = int.Parse(category) });


                    book.Authors = authorsList;
                    book.Categories = categoriesList;

                    HttpContent content = new StringContent(JsonConvert.SerializeObject(book), Encoding.UTF8, "application/json");
                    string endpoint = apiBaseUrl + $"Book/CreateBook";
                    using (var Response = await client.PostAsync(endpoint, content))
                    {
                        if (Response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            return await List();

                        }
                        else
                        {
                            ModelState.Clear();
                            //ModelState.AddModelError(string.Empty, "Username or Password is Incorrect");
                            return ErrorMessage();
                        }

                    }
                }
            }
        }

        public async Task<IActionResult> SaveBook()
        {
            var isbn = Request.Form["isbn"];
            var title = Request.Form["title"];
            var categories = Request.Form["categories"];
            var authors = Request.Form["authors"];
            var country = Request.Form["country"];
            var state= Request.Form["customSwitch3"];

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
                    string endpoint = apiBaseUrl + $"Book/UpdateBook";
                    using (var Response = await client.PostAsync(endpoint, content))
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
