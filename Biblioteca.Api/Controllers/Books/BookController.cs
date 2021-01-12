using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Biblioteca.Api.Resources.Books;
using Biblioteca.Api.Validators.Books;
using Biblioteca.Core.Models.Books;
using Biblioteca.Core.Services.Books;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;

namespace Biblioteca.Api.Controllers.Books
{
    [Route("{culture:culture}/api/[controller]")]
    [ApiController]
    public class BookController : Controller
    {

        // Dependency Injection
        private readonly IBookService _bookService;
        private readonly IBookCategoryService _bookCategoryService;
        private readonly IBookAuthorService _bookAuhtorService;
        private readonly IMapper _mapper;

        // Dependency Injection Localization
        private readonly IStringLocalizer<Localization.Controllers.Books.BookResource> _localizer;
        private readonly IStringLocalizer<Localization.Views.SharedResource> _sharedLocalizer;

        public BookController(IStringLocalizer<Localization.Controllers.Books.BookResource> localizer,
            IStringLocalizer<Localization.Views.SharedResource> SharedLocalizer,
            IBookService bookService, IBookCategoryService bookCategoryService,
            IBookAuthorService bookAuhtorService, IMapper mapper)
        {
            this._localizer = localizer;
            this._sharedLocalizer = SharedLocalizer;
            this._mapper = mapper;
            this._bookService = bookService;
            this._bookCategoryService = bookCategoryService;
            this._bookAuhtorService = bookAuhtorService;
        }

        #region LOCALIZATION

        [HttpGet("GetBookLanguageContent")]
        public IActionResult GetBookLanguageContent(string culture)
        {
            CultureInfo.CurrentCulture = new CultureInfo(culture);
            CultureInfo.CurrentUICulture = new CultureInfo(culture);
            var resourceSet = new List<Dictionary<string, string>>();

            resourceSet.Add(_localizer.GetAllStrings().ToDictionary(x => x.Name, x => x.Value));
            resourceSet.Add(_sharedLocalizer.GetAllStrings().ToDictionary(x => x.Name, x => x.Value));

            return Ok(resourceSet);
        }
        #endregion

        [HttpGet("GetAllWithCategoriesAndAuthor")]
        public async Task<ActionResult<IEnumerable<BookResource>>> GetAllWithCategoriesAndAuthor()
        {
            var books = await _bookService.GetAllWithCategoriesAndAuthor();
            var booksResource = _mapper.Map<IEnumerable<Book>, IEnumerable<BookResource>>(books);
            return Ok(booksResource);
        }

        [HttpGet("GetWithCategoriesAndAuthorById/{id}")]
        public async Task<ActionResult<BookResource>> GetWithCategoriesAndAuthorById(int id)
        {
            var book = await _bookService.GetWithCategoriesAndAuthorById(id);
            var bookResource = _mapper.Map<Book, BookResource>(book);
            return Ok(bookResource);
        }

        [HttpGet("GetAllByState/{state}")]
        public async Task<ActionResult<IEnumerable<Book>>> GetAllByState(bool state)
        {
            var book = await _bookService.GetAllByState(state);
            var bookResource = _mapper.Map<IEnumerable<Book>, IEnumerable<BookResource>>(book);
            return Ok(bookResource);
        }

        [HttpPost("CreateBook")]
        public async Task<ActionResult<BookResource>> CreateBook(SaveBookResource saveBookResource)
        {
            var validator = new SaveBookResourceValidator();
            var validationResult = await validator.ValidateAsync(saveBookResource);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var bookToCreate = _mapper.Map<SaveBookResource, Book>(saveBookResource);

            var newBook = await _bookService.CreateBook(bookToCreate);

            foreach (var author in saveBookResource.Authors)
            {
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

        [HttpPost("UpdateBook")]
        public async Task<ActionResult<BookResource>> UpdateBook(SaveBookResource saveBookResource)
        {
            var validator = new SaveBookResourceValidator();
            var validationResult = await validator.ValidateAsync(saveBookResource);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var bookToCreate = _mapper.Map<SaveBookResource, Book>(saveBookResource);

            await _bookService.UpdateBook(bookToCreate, bookToCreate);

            // Delete from reference table all Book-Authors
            var bookAuthorsToDelete = await _bookAuhtorService.GetBookAuthorById(saveBookResource.Id);

            foreach (var authorToDelete in bookAuthorsToDelete)
                await _bookAuhtorService.DeleteBookAuthor(authorToDelete);

            // Add to reference table all Book-Authors
            foreach (var author in saveBookResource.Authors)
            {
                BookAuthorResource bookAuthorResource = new BookAuthorResource();
                bookAuthorResource.AuthorId = author.Id;
                bookAuthorResource.BookId = saveBookResource.Id;

                var bookAuthorToAdd = _mapper.Map<BookAuthorResource, BookAuthor>(bookAuthorResource);
                var newBookAuthor = await _bookAuhtorService.CreateBookAuthor(bookAuthorToAdd);
            }


            // Delete from reference table all Book-Categories
            var bookCategoryToDelete = await _bookCategoryService.GetBookCategoryById(saveBookResource.Id);
            foreach (var categoryToDelete in bookCategoryToDelete)
                await _bookCategoryService.DeleteBookCategory(categoryToDelete);

            // Add to reference table all Book-Categories
            foreach (var category in saveBookResource.Categories)
            {
                BookCategoryResource bookCategoryResource = new BookCategoryResource();
                bookCategoryResource.CategoryId = category.Id;
                bookCategoryResource.BookId = saveBookResource.Id;

                var bookCategoryToCreate = _mapper.Map<BookCategoryResource, BookCategory>(bookCategoryResource);
                var newBookCategory = await _bookCategoryService.CreateBookCategory(bookCategoryToCreate);
            }

            var databaseBooks = await _bookService.GetWithCategoriesAndAuthorById(saveBookResource.Id);

            return Ok(_mapper.Map<Book, BookResource>(databaseBooks));
        }
    }
}
