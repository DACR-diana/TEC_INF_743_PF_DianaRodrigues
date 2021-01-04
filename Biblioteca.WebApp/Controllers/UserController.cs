using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

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

        private async Task<JsonResult> GetUser(int id)
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


        #region VIEWS

        public IActionResult Add()
        {
            return View();
        }
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
        #endregion
    }
}
