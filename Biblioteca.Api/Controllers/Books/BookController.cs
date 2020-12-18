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
    public class BookController : Controller
    { 
        
        // Dependency Injection
        private readonly IBookService _bookService;
        private readonly IMapper _mapper;

        public BookController(IBookService bookService, IMapper mapper)
        {
            this._mapper = mapper;
            this._bookService = bookService;
        }


        [HttpGet("")]
        public async Task<ActionResult<IEnumerable<BookResource>>> GetAllWithCategoriesAndAuthor()
        {
            var books = await _bookService.GetAllWithCategoriesAndAuthor();
            var booksResource = _mapper.Map<IEnumerable<Book>, IEnumerable<BookResource>>(books);
            return Ok(booksResource);
        }
    }
}
