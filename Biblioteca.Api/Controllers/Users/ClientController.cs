using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Biblioteca.Api.Resources.Users;
using Biblioteca.Api.Validators.Users;
using Biblioteca.Core.Models.Users;
using Biblioteca.Core.Services.Users;
using Microsoft.AspNetCore.Mvc;

namespace Biblioteca.Api.Controllers.Users
{
    [Route("{culture:culture}/api/[controller]")]
    [ApiController]
    public class ClientController : Controller
    {
        // Dependency Injection
        private readonly IClientService _clientService;
        private readonly IMapper _mapper;

        public ClientController(IClientService clientService, IMapper mapper)
        {
            this._mapper = mapper;
            this._clientService = clientService;
        }

        [HttpPost("CreateClient")]
        public async Task<ActionResult<ClientResource>> CreateClient(SaveClientResource saveClientResource)
        {
            var validator = new SaveClientResourceValidator();
            var validationResult = await validator.ValidateAsync(saveClientResource);
            // CHECK IS COMING OBJECT IS VALID
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            // MAPPING
            var clientToCreate = _mapper.Map<SaveClientResource, Client>(saveClientResource);

            // CREATE CLIENT
            var newClient = await _clientService.CreateClient(clientToCreate);

            // GET DATA FOR NEW CLIENT
            var databaseClient = await _clientService.GetWithCheckoutByEmail(newClient.Email);

            return Ok(_mapper.Map<Client, ClientResource>(databaseClient));
        }

        [HttpPost("UpdateClient")]
        public async Task<ActionResult<ClientResource>> UpdateClient(SaveClientResource saveClientResource)
        {
            var validator = new SaveClientResourceValidator();
            var validationResult = await validator.ValidateAsync(saveClientResource);

            // CHECK IS COMING OBJECT IS VALID
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            // MAPPING
            var clientToUpdate = _mapper.Map<SaveClientResource, Client>(saveClientResource);

            // GET CLIENT OBJECT BY EMAIL
            var clientDatabase = await _clientService.GetWithCheckoutByEmail(saveClientResource.Email);

            // SEND TO INTERFACE AND UPDATE CLIENT
            await _clientService.UpdateClient(clientDatabase, clientToUpdate);

            // GET NEW DATA FOR CLIENT
            clientDatabase = await _clientService.GetWithCheckoutByEmail(saveClientResource.Email);

            return Ok(_mapper.Map<Client, ClientResource>(clientDatabase));
        }

        [HttpGet("GetWithCheckoutByEmail/{email}")]
        public async Task<ActionResult<ClientResource>> GetWithCheckoutByEmail(string email)
        {
            // CALL INTERFACE  
            var client = await _clientService.GetWithCheckoutByEmail(email);

            // MAPPING OBJECTS
            var clientResource = _mapper.Map<Client, ClientResource>(client);
            return Ok(clientResource);
        }


        [HttpGet("GetAllWithCheckout")]
        public async Task<ActionResult<IEnumerable<ClientResource>>> GetAllWithCheckout()
        {
            // CALL INTERFACE  
            var clients = await _clientService.GetAllWithCheckout();


            // MAPPING OBJECTS
            var clientsResource = _mapper.Map<IEnumerable<Client>, IEnumerable<ClientResource>>(clients);
            return Ok(clientsResource);
        }

      

    }
}
