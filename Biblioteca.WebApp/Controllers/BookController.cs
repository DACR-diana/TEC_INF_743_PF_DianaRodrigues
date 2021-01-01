using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Biblioteca.WebApp.Models;
using Biblioteca.WebApp.Models.Books;
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
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Add()
        {
            var httpClientHandler = new HttpClientHandler();
            httpClientHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

            List<Category> categories = new List<Category>();
            List<Author> authors = new List<Author>();
            List<Country> countries = new List<Country>();

            using (HttpClient client = new HttpClient(httpClientHandler))
            {

                string endpoint = apiBaseUrl + $"Category/GetAllCategories";

                using (var Response = await client.GetAsync(endpoint))
                {
                    var result = await Response.Content.ReadAsStringAsync();
                    categories = JsonConvert.DeserializeObject<List<Category>>(result);
                }

                endpoint = apiBaseUrl + $"Author/GetAllAuthors";
                using (var Response = await client.GetAsync(endpoint))
                {
                    var result = await Response.Content.ReadAsStringAsync();
                    authors = JsonConvert.DeserializeObject<List<Author>>(result);
                }

                endpoint = apiBaseUrl + $"Country/GetAllCountries";
                using (var Response = await client.GetAsync(endpoint))
                {
                    var result = await Response.Content.ReadAsStringAsync();
                    countries = JsonConvert.DeserializeObject<List<Country>>(result);
                }
            }

            AddViewModel categoriesAuthors = new AddViewModel() { categories = categories, authors = authors, countries = countries };
            return View(categoriesAuthors);
        }

        public IActionResult List()
        {
            return View();
        }

        public async Task<IActionResult> AddBook()
        {
            var isbn = Request.Form["isbn"];
            var title = Request.Form["title"];
            var categories = Request.Form["categories"];
            var authors = Request.Form["authors"];
            var country = Request.Form["country"];

            if (categories.Count == 0 || authors.Count == 0 || country.Count == 0)
                return NotFound();
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
                            return View("List");
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
}
