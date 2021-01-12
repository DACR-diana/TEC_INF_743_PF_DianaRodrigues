using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Biblioteca.Api.Resources;
using Biblioteca.Core.Models;
using Biblioteca.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace Biblioteca.Api.Controllers
{
    [Route("{culture:culture}/api/[controller]")]
    [ApiController]
    public class PaymentController : Controller
    {// Dependency Injection
        private readonly IPaymentService _paymentService;
        private readonly IMapper _mapper;

        public PaymentController(IPaymentService paymentService, IMapper mapper)
        {
            this._mapper = mapper;
            this._paymentService = paymentService;
        }

        [HttpGet("GetAllPayments")]
        public async Task<ActionResult<IEnumerable<PaymentResource>>> GetAllPayments()
        {
            var payments = await _paymentService.GetAllPayments();
            var paymentsResource = _mapper.Map<IEnumerable<Payment>, IEnumerable<PaymentResource>>(payments);
            return Ok(paymentsResource);
        }
    }
}
