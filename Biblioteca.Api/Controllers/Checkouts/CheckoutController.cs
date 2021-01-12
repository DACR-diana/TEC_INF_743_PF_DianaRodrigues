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
    [Route("{culture:culture}/api/[controller]")]
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

        [HttpGet("GetWithCheckoutBooksById/{clientId}")]
        public ActionResult<CheckoutResource> GetWithCheckoutBooksById(int id)
        {
            var checkouts = _checkoutService.GetWithCheckoutBooksById(id);
            var checkoutsResource = _mapper.Map<List<Checkout>, List<CheckoutResource>>(checkouts);
            return Ok(checkoutsResource);
        }

        [HttpGet("GetWithCheckoutBooksByClientId/{clientId}")]
        public ActionResult<CheckoutResource> GetWithCheckoutBooksByClientId(int clientId)
        {
            var checkouts = _checkoutService.GetWithCheckoutBooksByClientId(clientId);
            var checkoutsResource = _mapper.Map<List<Checkout>, List<CheckoutResource>>(checkouts);
            return Ok(checkoutsResource);
        }

        [HttpGet("GetWithCheckoutBooksByClientIdAndState/{clientId}/{state}")]
        public ActionResult<CheckoutResource> GetWithCheckoutBooksByClientIdAndState(int clientId, bool state)
        {
            var checkouts = _checkoutService.GetWithCheckoutBooksByClientIdAndState(clientId, state);
            var checkoutsResource = _mapper.Map<List<Checkout>, List<CheckoutResource>>(checkouts);
            return Ok(checkoutsResource);
        }


        [HttpPost("CreateCheckout")]
        public async Task<ActionResult<List<CheckoutResource>>> CreateCheckout(SaveCheckoutResource saveCheckoutResource) // object coming from requests body
        {
            var validator = new SaveCheckoutResourceValidator();
            var validationResult = await validator.ValidateAsync(saveCheckoutResource);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var checkoutToCreate = _mapper.Map<SaveCheckoutResource, Checkout>(saveCheckoutResource);

            var newCheckout = _checkoutService.CreateCheckout(checkoutToCreate);

            var databaseCheckouts = _checkoutService.GetWithCheckoutBooksById(newCheckout[0].Id);

            return Ok(_mapper.Map<List<Checkout>, List<CheckoutResource>>(databaseCheckouts));
        }

        [HttpPost("UpdateCheckout")]
        public async Task<ActionResult<List<CheckoutResource>>> UpdateCheckout(SaveCheckoutResource saveCheckoutResource)
        {
            var validator = new SaveCheckoutResourceValidator();
            var validationResult = await validator.ValidateAsync(saveCheckoutResource);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var checkoutToUpdate = _mapper.Map<SaveCheckoutResource, Checkout>(saveCheckoutResource);

            var newCheckout = _checkoutService.UpdateCheckout(checkoutToUpdate);

            var databaseCheckouts = _checkoutService.GetWithCheckoutBooksById(newCheckout[0].Id);

            return Ok(_mapper.Map<List<Checkout>, List<CheckoutResource>>(databaseCheckouts));
        }

    }
}
