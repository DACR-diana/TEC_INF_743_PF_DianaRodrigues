using System;
using System.Collections.Generic;
using System.IO;
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
using IronPdf;
using System.Reflection.Metadata;
using jsreport.AspNetCore;
using jsreport.Types;

namespace Biblioteca.WebApp.Controllers
{
    public class CheckoutController : Controller
    {
        private string apiBaseUrl;
        private IConfiguration _Configure;
        private readonly IHttpClientHelper _clientClientHelper;
        public CheckoutController(IConfiguration configuration, IHttpClientHelper clientClientHelper, IJsReportMVCService jsReportMVCService)
        {
            _Configure = configuration;
            _clientClientHelper = clientClientHelper;
            JsReportMVCService = jsReportMVCService;
            apiBaseUrl = _Configure.GetValue<string>("WebAPIBaseUrl");
        }


        // Variable from JsReport function
        public IJsReportMVCService JsReportMVCService { get; }


        public IActionResult ErrorMessage()
        {
            return Json(new { Status = "Error" });
        }

        #region VIEWS

        // GO TO VIEW ADD
        public async Task<IActionResult> Add()
        {
            ViewBag.ClientName = HttpContext.Session.GetString("clientName");
            ViewBag.ClientEmail = HttpContext.Session.GetString("clientEmail");
            return View(await GetBooks());
        }

        // GO TO VIEW UPDATE
        public async Task<IActionResult> Update()
        {
            ViewBag.ClientName = HttpContext.Session.GetString("clientName");
            ViewBag.ClientEmail = HttpContext.Session.GetString("clientEmail");
            ViewBag.Books = await GetCheckoutsWithUserByClientId(int.Parse(HttpContext.Session.GetString("clientId")));
            return View();
        }

        // Function to redirect to view CheckoutUser 
        public async Task<IActionResult> CheckoutUser(int checkoutId)
        {
            var result = await GetWithCheckoutBooksById(checkoutId);
            var resultJSON = JsonConvert.SerializeObject(result.Value);
            var checkout = JsonConvert.DeserializeObject<Checkout>(resultJSON);

            ViewBag.ClientName = checkout.Client.Name;

            return View("CheckoutUser", checkout);
        }

        // GO TO CHECKOUT USER CHECKOUTS VIEW -> ALL CHECKOUTS PER USER
        public async Task<IActionResult> CheckoutUserCheckouts(int clientId = 0)
        {
            clientId = clientId == 0 ? int.Parse(HttpContext.Session.GetString("clientId")) : clientId;

            //var result = await GetCheckoutsWithUserByClientId(clientId);
            var result = await GetCheckoutsWithUserByClientIdByState(clientId, true);

            var resultJSON = JsonConvert.SerializeObject(result.Value);
            var checkouts = JsonConvert.DeserializeObject<List<Checkout>>(resultJSON);

            ViewBag.ClientId = clientId;
            ViewBag.ClientName = HttpContext.Session.GetString("clientName");

            return View("CheckoutUserCheckouts", result);
        }

        // GO TO CHECKOUT LIST USER -> ALL USERS
        public async Task<IActionResult> CheckoutListUser()
        {
            ViewBag.Users = await GetClients();
            return View("CheckoutListUser");
        }

        #endregion

        #region GET
        // Function to get all clients
        private async Task<JsonResult> GetClients()
        {

            var result = await _clientClientHelper.GetContent($"{apiBaseUrl}{ HttpContext.Session.GetString("language")}/api/Client/GetAllWithCheckout");
            var jsonResult = await result.Content.ReadAsStringAsync();
            var clients = JsonConvert.DeserializeObject<List<Client>>(jsonResult);
            return new JsonResult(clients);
        }

        // Function to get all books
        private async Task<JsonResult> GetBooks()
        {
            List<Book> books = new List<Book>();

            // API CALL
            string endpoint = $"{apiBaseUrl}{ HttpContext.Session.GetString("language")}/api/Book/GetAllByState/{true}";
            var response = await _clientClientHelper.GetContent(endpoint);

            var result = await response.Content.ReadAsStringAsync();

            // Convert string to List of Objects
            books = JsonConvert.DeserializeObject<List<Book>>(result);
            return new JsonResult(books);
        }

        // Function to get all books from one checkout
        private async Task<JsonResult> GetWithCheckoutBooksById(int id)
        {

            var result = await _clientClientHelper.GetContent($"{apiBaseUrl}{ HttpContext.Session.GetString("language")}/api/Checkout/GetWithCheckoutBooksById/{id}");
            var resultJson = await result.Content.ReadAsStringAsync();
            var checkouts = JsonConvert.DeserializeObject<Checkout>(resultJson);
            return new JsonResult(checkouts);
        }

        // Function to get all checkouts from one user
        private async Task<JsonResult> GetCheckoutsWithUserByClientId(int clientId)
        {
            var checkouts = new List<Checkout>();

            string endpoint = $"{apiBaseUrl}{ HttpContext.Session.GetString("language")}/api/Checkout/GetWithCheckoutBooksByClientId/{clientId}";
            var Response = await _clientClientHelper.GetContent(endpoint);
            var result = await Response.Content.ReadAsStringAsync();
            checkouts = JsonConvert.DeserializeObject<List<Checkout>>(result);
            return new JsonResult(checkouts);
        }

        // Get all checkouts from one client depending on the id and state
        public async Task<JsonResult> GetCheckoutsWithUserByClientIdByState(int clientId, bool state)
        {
            var checkouts = new List<Checkout>();

            string endpoint = $"{apiBaseUrl}{ HttpContext.Session.GetString("language")}/api/Checkout/GetWithCheckoutBooksByClientIdAndState/{clientId}/{state}";
            var Response = await _clientClientHelper.GetContent(endpoint);
            var result = await Response.Content.ReadAsStringAsync();
            checkouts = JsonConvert.DeserializeObject<List<Checkout>>(result);
            return new JsonResult(checkouts);
        }

        // FUNCTION TO CONSTRUCT HTML TABLE
        public async Task<JsonResult> FilterTable(int clientID, bool filter)
        {

            var checkouts = new List<Checkout>();

            // GET DATA
            var result = await GetCheckoutsWithUserByClientIdByState(clientID, filter);

            // CONVERT RESPONSE TO OBJECT
            var resultJSON = JsonConvert.SerializeObject(result.Value);
            checkouts = JsonConvert.DeserializeObject<List<Checkout>>(resultJSON);

            // HTML FOR TABLE
            string html = @"<thead>
                        <tr>
                         <th>Id Checkout</th>
                         <th>Data</th>
                         <th>Data de Entrega</th>
                         <th>Estado</th>
                        </tr>
                    </thead>
                    <tbody>";

            foreach (var checkout in checkouts)
            {
                html += @"<tr onclick=location.href='/Checkout/CheckoutUser?checkoutId="+ checkout.Id +"'><td> " + checkout.Id + " </td><td> " + checkout.Date + " </td><td> " + checkout.ExpectedDate + " </td>";


                if (checkout.DeliveryDate != null)
                    html += " <td><span class='badge bg-primary'>Encerrado</span></td>";
                else if (checkout.ExpectedDate < DateTime.Now)
                    html += "<td> <span class='badge bg-danger'>Multa Aplicada</span></td>";
                else
                    html += "<td><span class='badge bg-success'>A decorrer</span></td>";

                html += "</tr>";
            }

            html += @"</tbody>";

            return new JsonResult(html);
        }


        #endregion

        #region Actions

        // Create checkout function
        public async Task<IActionResult> CreateCheckout()
        {
            var books = Request.Form["books"];

            // Check Book Count -> IF 0 Show to erro message
            if (books.Count == 0)
                return ErrorMessage();
            else
            {
                Checkout checkout = new Checkout();
                checkout.Date = DateTime.Now;
                checkout.ClientId = int.Parse(HttpContext.Session.GetString("clientId"));
                checkout.ExpectedDate = DateTime.Now.AddDays(7);

                List<CheckoutBook> checkoutBooks = new List<CheckoutBook>();
                // Add books to checkout object
                foreach (var book in books)
                {
                    var bookForCheckout = new Book() { Id = int.Parse(book) };
                    checkoutBooks.Add(new CheckoutBook() { Book = bookForCheckout });
                }


                checkout.CheckoutBooks = checkoutBooks;

                // API CALL
                HttpContent content = new StringContent(JsonConvert.SerializeObject(checkout), Encoding.UTF8, "application/json");
                string endpoint = $"{apiBaseUrl}{ HttpContext.Session.GetString("language")}/api/Checkout/CreateCheckout";
                var response = await _clientClientHelper.PostContent(endpoint, content);

                // Redirect to View CheckoutUser
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var resultJson = await response.Content.ReadAsStringAsync();
                    Checkout newCheckout = JsonConvert.DeserializeObject<Checkout>(resultJson);

                    return await CheckoutUser(newCheckout.Id);
                }
                else
                {
                    ModelState.Clear();
                    return ErrorMessage();
                }


            }
        }

        // Update Checkout
        public async Task<IActionResult> SaveCheckout(int checkoutId)
        {
            // Get Checkout Object from DataBase
            var result = await GetWithCheckoutBooksById(checkoutId);

            // If it's null return error message
            if (result.Value == null)
                return ErrorMessage();
            else
            {
                // Else convert Json to Object
                var resultJSON = JsonConvert.SerializeObject(result.Value);
                var checkout = JsonConvert.DeserializeObject<Checkout>(resultJSON);

                // API CALL
                HttpContent content = new StringContent(JsonConvert.SerializeObject(checkout), Encoding.UTF8, "application/json");
                string endpoint = $"{apiBaseUrl}{ HttpContext.Session.GetString("language")}/api/Checkout/UpdateCheckout";
                var teste = JsonConvert.SerializeObject(checkout);
                // API RESPONSE
                var newResult = await _clientClientHelper.PostContent(endpoint, content);

                // If it's all ok, return code 200 
                if (newResult.StatusCode == System.Net.HttpStatusCode.OK)
                    return Json(new { Status = "200", Result = newResult.Content.ReadAsStringAsync() });
                else
                {
                    ModelState.Clear();
                    return ErrorMessage();
                }
            }

        }
        #endregion

        #region PDF

        // Generate PDF and download
        [MiddlewareFilter(typeof(JsReportPipeline))]
        public async Task<IActionResult> CreatePDF(int checkoutId)
        {
            // CALL TO JsReport Package
            HttpContext.JsReportFeature().Recipe(Recipe.ChromePdf)
                .OnAfterRender((r) => HttpContext.Response.Headers["Content-Disposition"] = "attachment; filename=\"Report.pdf\"");

            // GET DATA
            var result = await GetWithCheckoutBooksById(checkoutId);
            var resultJSON = JsonConvert.SerializeObject(result.Value);
            var checkout = JsonConvert.DeserializeObject<Checkout>(resultJSON);

            ViewBag.ClientName = checkout.Client.Name;

            // GO TO VIEW INVOICE TO DOWNLOAD
            return View("PDF/Invoice", checkout);
        }
        #endregion
    }
}
