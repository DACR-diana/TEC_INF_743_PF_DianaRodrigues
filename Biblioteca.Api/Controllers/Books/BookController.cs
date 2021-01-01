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
        private readonly IBookCategoryService _bookCategoryService;
        private readonly IBookAuthorService _bookAuhtorService;
        private readonly IMapper _mapper;

        public BookController(IBookService bookService, IBookCategoryService bookCategoryService,
            IBookAuthorService bookAuhtorService, IMapper mapper)
        {
            this._mapper = mapper;
            this._bookService = bookService;
            this._bookCategoryService = bookCategoryService;
            this._bookAuhtorService = bookAuhtorService;
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

            foreach(var author in saveBookResource.Authors){
                BookAuthorResource bookAuthorResource = new BookAuthorResource();
                bookAuthorResource.AuthorId = author.Id;
                bookAuthorResource.BookId = newBook.Id;

                var bookAuthorToCreate = _mapper.Map<BookAuthorResource, BookAuthor>(bookAuthorResource);
                var newBookAuhtor = await _bookAuhtorService.CreateBookAuthor(bookAuthorToCreate);
            }

            foreach (var category in saveBookResource.Categories)
            {
                BookCategoryResource bookCategoryResource = new BookCategoryResource();
                bookCategoryResource.CategoryId = category.Id;
                bookCategoryResource.BookId = newBook.Id;

                var bookCategoryToCreate = _mapper.Map<BookCategoryResource, BookCategory>(bookCategoryResource);
                var newBookCategory = await _bookCategoryService.CreateBookCategory(bookCategoryToCreate);
            }

            var databaseBooks = await _bookService.GetWithCategoriesAndAuthorById(newBook.Id);

            return Ok(_mapper.Map<Book, BookResource>(databaseBooks));
        }
    }
}
