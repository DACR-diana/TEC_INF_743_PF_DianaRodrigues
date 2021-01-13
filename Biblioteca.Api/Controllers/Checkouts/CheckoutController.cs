using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Biblioteca.Api.Resources;
using Biblioteca.Api.Resources.Checkouts;
using Biblioteca.Api.Validators.Checkouts;
using Biblioteca.Core.Models;
using Biblioteca.Core.Models.Checkouts;
using Biblioteca.Core.Services;
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
        private readonly ITicketService _ticketService;
        private readonly IMapper _mapper;

        public CheckoutController(ICheckoutService checkoutService, ITicketService ticketService, IMapper mapper)
        {
            this._mapper = mapper;
            this._checkoutService = checkoutService;
            this._ticketService = ticketService;
        }

        [HttpGet("GetWithCheckoutBooksById/{id}")]
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

        [HttpGet("GetExpiredCheckoutsAndApplyTicket")]
        public async Task<ActionResult<List<CheckoutResource>>> GetExpiredCheckoutsAndApplyTicket()
        {
            //Get Experid Checkouts
            var expiredCheckouts = _checkoutService.GetExpiredCheckouts();

            // For each checkout insert ticket. If checkout already has a ticket then do nothing
            var expiredCheckoutsResource = _mapper.Map<List<Checkout>, List<CheckoutResource>>(expiredCheckouts);


            foreach (var expiredCheckoutResource in expiredCheckoutsResource)
            {
                var ticketCheckout = await _ticketService.GetAllWithCheckoutsByCheckoutsId(expiredCheckoutResource.Id);

                if (ticketCheckout == null)
                {
                    SaveTicketResource saveTicketResource = new SaveTicketResource();
                    saveTicketResource.CheckoutId = expiredCheckoutResource.Id;
                    saveTicketResource.Date = DateTime.Now;
                    saveTicketResource.PaymentId = 1;
                    saveTicketResource.Price = 5;
                    saveTicketResource.State = true;

                    var saveTicket = _mapper.Map<SaveTicketResource, Ticket>(saveTicketResource);
                    var newTicket = await _ticketService.CreateTicket(saveTicket);
                }
            }
            return Ok(expiredCheckoutsResource);
        }

        [HttpGet("GetExpiredCheckoutsAndApplyTicketByCheckoutId/{checkoutId}")]
        public async Task<ActionResult<CheckoutResource>> GetExpiredCheckoutsAndApplyTicketByCheckoutId(int checkoutId)
        {
            //Get Experid Checkout
            var expiredCheckout = _checkoutService.GetExpiredCheckoutById(checkoutId);
            var expiredCheckoutResource = _mapper.Map<Checkout, CheckoutResource>(expiredCheckout);

            var ticketCheckout = await _ticketService.GetAllWithCheckoutsByCheckoutsId(expiredCheckoutResource.Id);

            if (ticketCheckout == null)
            {
                SaveTicketResource saveTicketResource = new SaveTicketResource();
                saveTicketResource.CheckoutId = expiredCheckout.Id;
                saveTicketResource.Date = DateTime.Now;
                saveTicketResource.PaymentId = 1;
                saveTicketResource.Price = 5;
                saveTicketResource.State = true;

                var saveTicket = _mapper.Map<SaveTicketResource, Ticket>(saveTicketResource);
                var newTicket = await _ticketService.CreateTicket(saveTicket);
            }
            return Ok(expiredCheckoutResource);
        }

        [HttpGet("GetExpiredCheckouts")]
        public ActionResult<List<CheckoutResource>> GetExpiredCheckouts()
        {
            var expiredCheckouts = _checkoutService.GetExpiredCheckouts();
            var expiredCheckoutsResource = _mapper.Map<List<Checkout>, List<CheckoutResource>>(expiredCheckouts);
            return Ok(expiredCheckoutsResource);
        }


        [HttpPost("CreateCheckout")]
        public async Task<ActionResult<CheckoutResource>> CreateCheckout(SaveCheckoutResource saveCheckoutResource)
        {
            var validator = new SaveCheckoutResourceValidator();
            var validationResult = await validator.ValidateAsync(saveCheckoutResource);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var checkoutToCreate = _mapper.Map<SaveCheckoutResource, Checkout>(saveCheckoutResource);

            var newCheckout = _checkoutService.CreateCheckout(checkoutToCreate);

            return Ok(_mapper.Map<Checkout, CheckoutResource>(newCheckout));
        }

        [HttpPost("UpdateCheckout")]
        public async Task<IActionResult> UpdateCheckout(SaveCheckoutResource saveCheckoutResource)
        {
            var validator = new SaveCheckoutResourceValidator();
            var validationResult = await validator.ValidateAsync(saveCheckoutResource);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var checkoutToUpdate = _mapper.Map<SaveCheckoutResource, Checkout>(saveCheckoutResource);

            var newCheckout = _checkoutService.UpdateCheckout(checkoutToUpdate);

            // Checkout If Checkout Is Expired And Ticketed
            var expiredCheckout = await GetExpiredCheckoutsAndApplyTicketByCheckoutId(checkoutToUpdate.Id);

            //New Ticket with updated data
            SaveTicketResource saveTicketResource = new SaveTicketResource();
            saveTicketResource.CheckoutId = checkoutToUpdate.Id;
            saveTicketResource.State = false;
            saveTicketResource.PaymentDate = DateTime.Now;

            //Get Old Ticket
            var oldTicket = await _ticketService.GetAllWithCheckoutsByCheckoutsId(checkoutToUpdate.Id);

            //UpdateTicket
            var newTicket = _mapper.Map<SaveTicketResource, Ticket>(saveTicketResource);
            await _ticketService.UpdateTicket(oldTicket, newTicket);

            return Ok();
        }

    }
}
