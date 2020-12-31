using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Biblioteca.Api.Resources.Books;
using Biblioteca.Api.Validators.Books;
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

        [HttpPost("CreateBook")]
        public async Task<ActionResult<BookResource>> CreateBook(SaveBookResource saveBookResource) // object coming from requests body
        {
            var validator = new SaveBookResourceValidator();
            var validationResult = await validator.ValidateAsync(saveBookResource);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var bookToCreate = _mapper.Map<SaveBookResource, Book>(saveBookResource);

            var newBook = await _bookService.CreateBook(bookToCreate);

            var databaseBooks = await _bookService.GetWithCategoriesAndAuthorById(newBook.Id);


            return Ok(_mapper.Map<Book, BookResource>(databaseBooks));
        }
    }
}
