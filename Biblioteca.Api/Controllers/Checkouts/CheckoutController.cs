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
        public async Task<ActionResult<CheckoutResource>> GetWithCheckoutBooksById(int id)
        {
            var checkouts = _checkoutService.GetWithCheckoutBooksById(id);

            var checkoutsResource = _mapper.Map<Checkout, CheckoutResource>(checkouts);

            var ticketCheckout = await _ticketService.GetAllWithCheckoutsByCheckoutsId(checkoutsResource.Id);

            var newTicketResource = _mapper.Map<Ticket, TicketResource>(ticketCheckout);
            checkoutsResource.Tickets.Add(newTicketResource);

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

        // FUNCTION FOR INTERNAL USE ONLY
        private async Task<Ticket> GetExpiredCheckoutsAndApplyTicketByCheckoutId(int checkoutId,bool isCheckoutJustUpdated)
        {

            // THIS FUNCTION IS NEEDED WHEN SOME CHECKOUT SUFFERS AN UPDATE OR A FUNCTION NEEDS TO KNOW IF CHECKOUT IS EXPIRED
            // BECAUSE IF SO, A NEW TICKET NEEDS TO BE INSERTED IN THE DATABASE 

            Checkout expiredCheckout = new Checkout();


            //Check If Checkout Is Expired
            expiredCheckout = _checkoutService.GetExpiredCheckoutById(checkoutId);

            // Check if the expired checkout has a ticket applied
            var ticketCheckout = await _ticketService.GetAllWithCheckoutsByCheckoutsIdAndState(checkoutId, true);

            // If the expired checkout has no ticket applied, then create ticket
            if (ticketCheckout == null && expiredCheckout.Id != 0)
            {
                SaveTicketResource saveTicketResource = new SaveTicketResource();
                saveTicketResource.CheckoutId = expiredCheckout.Id;
                saveTicketResource.Date = DateTime.Now;
                saveTicketResource.PaymentId = 1;
                saveTicketResource.Price = 5;
                saveTicketResource.State = true;

                var saveTicket = _mapper.Map<SaveTicketResource, Ticket>(saveTicketResource);
                ticketCheckout = await _ticketService.CreateTicket(saveTicket);
            }
            else
                expiredCheckout.Id = checkoutId;


            // If this parameter is true, then close the ticket
            if (isCheckoutJustUpdated && ticketCheckout != null)
            {
                ticketCheckout.PaymentDate = DateTime.Now;
                ticketCheckout.State = false;

                await _ticketService.UpdateTicket(ticketCheckout);
            }
            
            return ticketCheckout;

        }

        [HttpGet("GetExpiredCheckouts")]
        public ActionResult<List<CheckoutResource>> GetExpiredCheckouts()
        {
            var expiredCheckouts = _checkoutService.GetExpiredCheckouts();
            var expiredCheckoutsResource = _mapper.Map<List<Checkout>, List<CheckoutResource>>(expiredCheckouts);
            return Ok(expiredCheckoutsResource);
        }


        [HttpPost("CreateCheckout")]
        public async Task<CheckoutResource> CreateCheckout(SaveCheckoutResource saveCheckoutResource)
        {

            try
            {
                var validator = new SaveCheckoutResourceValidator();
                var validationResult = await validator.ValidateAsync(saveCheckoutResource);

                if (!validationResult.IsValid)
                    return new CheckoutResource();

                var checkoutToCreate = _mapper.Map<SaveCheckoutResource, Checkout>(saveCheckoutResource);

                var newCheckout = _checkoutService.CreateCheckout(checkoutToCreate);

                return _mapper.Map<Checkout, CheckoutResource>(newCheckout);
            }
            catch(Exception ex)
            {
                return new CheckoutResource();
            }
            
        }

        [HttpPost("UpdateCheckout")]
        public async Task<IActionResult> UpdateCheckout(SaveCheckoutResource saveCheckoutResource)
        {
            try
            {
                SaveCheckoutResourceValidator validator = new SaveCheckoutResourceValidator();
                var validationResult = await validator.ValidateAsync(saveCheckoutResource);

                // Validate if object is well constructed
                if (!validationResult.IsValid)
                    return BadRequest(validationResult.Errors);

                // Mapping 
                Checkout checkoutToUpdate = _mapper.Map<SaveCheckoutResource, Checkout>(saveCheckoutResource);


                // Checkout If Checkout Is Expired And Ticketed
                // Update's the Ticket as well if the second parameter is true
                Ticket oldTicket = await GetExpiredCheckoutsAndApplyTicketByCheckoutId(checkoutToUpdate.Id, true);

                //Update And Return New Checkout
                Checkout newCheckout = _checkoutService.UpdateCheckout(checkoutToUpdate);

                // Return updated checkout
                List<Ticket> checkoutTickets = new List<Ticket>();

                // If there is no ticket, there is no need to adde to checkout object
                if(oldTicket!=null)
                {
                    checkoutTickets.Add(oldTicket);
                    newCheckout.Tickets = checkoutTickets;
                }
              

                return Ok(newCheckout);
            }
            catch (Exception ex)
            {
                return NotFound(ex);
            }

        }

        [HttpGet("GetDashboardInformation")]
        public List<int> GetDashboardInformation()
        {
            return _checkoutService.GetDashboardInformationThroughStoredProcedure();
        }

    }
}
