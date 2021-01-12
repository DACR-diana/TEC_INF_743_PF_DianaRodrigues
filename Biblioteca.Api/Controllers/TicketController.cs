using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Biblioteca.Api.Controllers
{
    [Route("{culture:culture}/api/[controller]")]
    [ApiController]
    public class TicketController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
