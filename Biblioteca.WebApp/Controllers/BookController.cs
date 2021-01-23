using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Biblioteca.WebApp.Helpers;
using Biblioteca.WebApp.Models;
using Biblioteca.WebApp.Models.Books;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;

namespace Biblioteca.WebApp.Controllers
{
    public class BookController : Controller
    {
        private string apiBaseUrl;
        private IConfiguration _Configure;
        private readonly IHttpClientHelper _clientClientHelper;

        public BookController(IConfiguration configuration, IHttpClientHelper clientClientHelper)
        {
            _Configure = configuration;
            _clientClientHelper = clientClientHelper;

            apiBaseUrl = _Configure.GetValue<string>("WebAPIBaseUrl");

        }

        public IActionResult ErrorMessage()
        {
            return Content("Ocorreu algum erro");
        }


        //#region LOCALIZATION
        //private async Task<IActionResult> GetContent(string lang)
        //{
        //    var request = new HttpRequestMessage(HttpMethod.Get, $"{apiBaseUrl}{lang}/Book/GetBookLanguageContent");
        //    var client = _clientFactory.CreateClient();
        //    var response = await client.SendAsync(request);

        //    if (response.StatusCode == System.Net.HttpStatusCode.OK)
        //    {
        //        var responseStream = response.Content.ReadAsStreamAsync();
        //        return Ok(responseStream);
        //    }
        //    else
        //        return ErrorMessage();
        //}
        //#endregion

        #region GET

        private async Task<List<Country>> GetCountries()
        {
            var httpClientHandler = new HttpClientHandler();
            httpClientHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

            List<Country> countries = new List<Country>();

            using (HttpClient client = new HttpClient(httpClientHandler))
            {

                string endpoint = apiBaseUrl + $"{ HttpContext.Session.GetString("language")}/api/Country/GetAllCountries";
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

                string endpoint = apiBaseUrl + $"{ HttpContext.Session.GetString("language")}/api/Author/GetAllAuthors";
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

                string endpoint = $"{apiBaseUrl}{ HttpContext.Session.GetString("language")}/api/Category/GetAllCategories";
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
                string endpoint = $"{apiBaseUrl}{ HttpContext.Session.GetString("language")}/api/Book/GetAllByState/{true}";
                //string endpoint = apiBaseUrl + $"Book/GetAllWithCategoriesAndAuthor";
                using (var Response = await client.GetAsync(endpoint))
                {
                    var result = await Response.Content.ReadAsStringAsync();
                    books = JsonConvert.DeserializeObject<List<Book>>(result);
                }

            }
            return new JsonResult(books);
        }


        // FUNCTION TO CONSTRUCT HTML TABLE
        public async Task<JsonResult> FilterTable(string filter)
        {


            var books = new List<Book>();

            // API CALL
            string endpoint = $"{apiBaseUrl}{ HttpContext.Session.GetString("language")}/api/Book/GetAllByState/{filter}";
            using (var Response = await _clientClientHelper.GetContent(endpoint))
            {
                var result = await Response.Content.ReadAsStringAsync();
                books = JsonConvert.DeserializeObject<List<Book>>(result);
            }

            string html = @"<thead>
                        <tr>
                            <th>Id</th>
                            <th>ISBN</th>
                            <th>Titulo</th>
                            <th>País</th>
                            <th>Estado</th>
                        </tr>
                    </thead>
                    <tbody>";

            foreach (var book in books)
            {
                html += @"<tr onclick=location.href='/Book/Update/" + book.Id + "'><td> " + book.Id + " </td><td> " + book.ISBN + " </td><td> " + book.Title + " </td><td> " + book.Country.Name + " </td>";


                html += book.State == true ? "<td> <span class='badge badge-success'>Ativo</span></td>" : "<td><span class='badge badge-danger'>Inativo</span></td>";
                html += "</tr>";
            }

            html += @"</tbody>";

            return new JsonResult(html);
        }

        // GET BOOK BY ID
        private async Task<JsonResult> GetBook(int id)
        {
            Book book = new Book();

            // API CALL
            string endpoint = $"{apiBaseUrl}{ HttpContext.Session.GetString("language")}/api/Book/GetWithCategoriesAndAuthorById/{id}";

            using (var Response = await _clientClientHelper.GetContent(endpoint))
            {
                var result = await Response.Content.ReadAsStringAsync();
                book = JsonConvert.DeserializeObject<Book>(result);
            }
            return new JsonResult(book);
        }

        // CHECK IF BOOK BY ISBN ALREADY EXISTS
        public async Task<IActionResult> GetBookByISBN(string ISBN)
        {
            List<Book> book = new List<Book>();

            // API CALL
            string endpoint = $"{apiBaseUrl}{ HttpContext.Session.GetString("language")}/api/Book/GetBookByISBN/{ISBN}";

            var response = await _clientClientHelper.GetContent(endpoint);
            var result = await response.Content.ReadAsStringAsync();
            book = JsonConvert.DeserializeObject<List<Book>>(result);

            //  IF THE OBJECT BOOK US NULL RETURN FORBIDDEN CODE
            if (book.Count>0)
                return Json(new { Status = "403" });
            else
                return Json(new { Status = "200" });
        }

        #endregion

        #region Views
        // UPDATE BOOK VIEW
        public async Task<IActionResult> Update(int id)
        {
            ViewBag.Book = await GetBook(id);
            HttpContext.Session.SetString("bookId", id.ToString());
            AddViewModel categoriesAuthors = new AddViewModel()
            {
                categories = await GetCategories(),
                authors = await GetAuthors(),
                countries = await GetCountries()
            };
            return View(categoriesAuthors);
        }

        // LIST VIEW
        public async Task<IActionResult> List()
        {
            ViewBag.Books = await GetBooks();
            return View("List");
        }


        // LOAD CATEGORIES,AUTHORS AND COUNTRIES AND GO TO ADD VIEW
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

        #endregion

        #region Actions

        // ADD BOOK FUNCTION
        public async Task<IActionResult> AddBook()
        {
            // REQUEST DATA FROM FORM
            var isbn = Request.Form["isbn"];
            var title = Request.Form["title"];
            var categories = Request.Form["categories"];
            var authors = Request.Form["authors"];
            var country = Request.Form["country"];

            if (categories.Count == 0 || authors.Count == 0 || country.Count == 0)
                return ErrorMessage();
            else
            {
                // CREATE BOOK OBJECT
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

                // API CALL
                HttpContent content = new StringContent(JsonConvert.SerializeObject(book), Encoding.UTF8, "application/json");
                string endpoint = $"{apiBaseUrl}{ HttpContext.Session.GetString("language")}/api/Book/CreateBook";
                using (var Response = await _clientClientHelper.PostContent(endpoint, content))
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

        // SAVE BOOK
        public async Task<IActionResult> SaveBook()
        {
            // REQUEST VALUES FROM FORM
            var isbn = Request.Form["isbn"];
            var title = Request.Form["title"];
            var categories = Request.Form["categories"];
            var authors = Request.Form["authors"];
            var country = Request.Form["country"];
            var state = Request.Form["customSwitch3"];

            // CHECBOX WHEN ACTIVATED COMES WITH 2 VALUES, SO IF HAS 2 VALUES, THE VALUE IS TRUE AND THE CODE CAN DISCARD THE SECOND VALUE
            if (Request.Form["customSwitch3"].Count > 1)
                state = Request.Form["customSwitch3"][0];

            if (categories.Count == 0 || authors.Count == 0 || country.Count == 0)
                return ErrorMessage();
            else
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
                string endpoint = $"{apiBaseUrl}{ HttpContext.Session.GetString("language")}/api/Book/UpdateBook";
                using (var Response = await _clientClientHelper.PostContent(endpoint, content))
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
        #endregion
    }
}
