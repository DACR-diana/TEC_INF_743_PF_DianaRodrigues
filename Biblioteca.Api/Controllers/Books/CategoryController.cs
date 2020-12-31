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
    public class CategoryController : Controller
    {
        // Dependency Injection
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;

        public CategoryController(ICategoryService categoryService, IMapper mapper)
        {
            this._mapper = mapper;
            this._categoryService = categoryService;
        }
      

        [HttpGet("GetAllCategories")]
        public async Task<ActionResult<IEnumerable<CategoryResource>>> GetAllCategories()
        {
            var categories = await _categoryService.GetAllCategories();
            var categoriesResource = _mapper.Map<IEnumerable<Category>, IEnumerable<CategoryResource>>(categories);
            return Ok(categoriesResource);
        }
    }
}
