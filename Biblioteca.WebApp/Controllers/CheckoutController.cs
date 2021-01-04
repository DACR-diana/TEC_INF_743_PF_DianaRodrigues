using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Biblioteca.WebApp.Controllers
{
    public class CheckoutController : Controller
    {

        #region VIEWS

        public IActionResult Add()
        {
            return View();
        }

        #endregion
    }
}
