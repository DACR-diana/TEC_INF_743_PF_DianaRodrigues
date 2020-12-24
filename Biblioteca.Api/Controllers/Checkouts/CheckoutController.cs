using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Biblioteca.Api.Resources.Checkouts;
using Biblioteca.Api.Validators.Checkouts;
using Biblioteca.Core.Models.Checkouts;
using Biblioteca.Core.Services.Books;
using Biblioteca.Core.Services.Checkouts;
using Microsoft.AspNetCore.Mvc;

namespace Biblioteca.Api.Controllers.Checkouts
{
    [Route("api/[controller]")]
    [ApiController]
    public class CheckoutController : Controller
    {

        // Dependency Injection
        private readonly ICheckoutService _checkoutService;
        private readonly IMapper _mapper;

        public CheckoutController(ICheckoutService checkoutService, IMapper mapper)
        {
            this._mapper = mapper;
            this._checkoutService = checkoutService;
        }


        [HttpGet("")]
        public async Task<ActionResult<IEnumerable<CheckoutResource>>> GetAllWithClientsAndBook()
        {
            var checkouts = await _checkoutService.GetAllWithClientsAndBook();
            var checkoutsResource = _mapper.Map<IEnumerable<Checkout>, IEnumerable<CheckoutResource>>(checkouts);
            return Ok(checkoutsResource);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<CheckoutResource>>> GetWithUserAndBookById(int id)
        {
            var checkouts = await _checkoutService.GetWithUserAndBookById(id);
            var checkoutsResource = _mapper.Map<IEnumerable<Checkout>, IEnumerable<CheckoutResource>>(checkouts);
            return Ok(checkoutsResource);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<CheckoutResource>>> GetAllWithUserAndBookByUserId(int userId)
        {
            var checkouts = await _checkoutService.GetAllWithUserAndBookByUserId(userId);
            var checkoutsResource = _mapper.Map<IEnumerable<Checkout>, IEnumerable<CheckoutResource>>(checkouts);
            return Ok(checkoutsResource);
        }

        [HttpPost("")]
        public async Task<ActionResult<CheckoutResource>> CreateCheckout([FromBody] SaveCheckoutResource saveCheckoutResource) // object coming from requests body
        {
            var validator = new SaveCheckoutResourceValidator();
            var validationResult = await validator.ValidateAsync(saveCheckoutResource);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors); 

            var checkoutToCreate = _mapper.Map<SaveCheckoutResource, Checkout>(saveCheckoutResource);

            var newCheckout = await _checkoutService.CreateCheckout(checkoutToCreate);

            var databaseCheckouts = await _checkoutService.GetWithUserAndBookById(newCheckout.Id);

            var checkoutResource= new List<CheckoutResource>();

            foreach (var checkout in databaseCheckouts)
                checkoutResource.Add(_mapper.Map<Checkout, CheckoutResource>(checkout));

            return Ok(checkoutResource);
        }

    }
}
