using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Biblioteca.Api.Resources.Books;
using Biblioteca.Core.Models.Books;
using Biblioteca.Core.Services.Books;
using Microsoft.AspNetCore.Mvc;

namespace Biblioteca.Api.Controllers.Books
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : Controller
    {

        // Dependency Injection
        private readonly IAuthorService _auhtorService;
        private readonly IMapper _mapper;
        
        public AuthorController(IAuthorService auhtorService, IMapper mapper)
        {
            this._mapper = mapper;
            this._auhtorService = auhtorService;
        }



        [HttpGet("")]
        public async Task<ActionResult<IEnumerable<AuthorResource>>> GetAllAuthors()
        {
            var authors = await _auhtorService.GetAllAuthors();
            var authorsResource = _mapper.Map<IEnumerable<Author>, IEnumerable<AuthorResource>>(authors);
            return Ok(authorsResource);
        }
    }
}
