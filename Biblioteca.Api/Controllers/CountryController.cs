using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Biblioteca.Api.Resources;
using Biblioteca.Api.Resources.Books;
using Biblioteca.Core.Models;
using Biblioteca.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace Biblioteca.Api.Controllers
{
    [Route("{culture:culture}/api/[controller]")]
    [ApiController]
    public class CountryController : Controller
    {
        // Dependency Injection
        private readonly ICountryService _countryService;
        private readonly IMapper _mapper;

        public CountryController(ICountryService countryService, IMapper mapper)
        {
            this._mapper = mapper;
            this._countryService = countryService;
        }

        [HttpGet("GetAllCountries")]
        public async Task<ActionResult<IEnumerable<CountryResource>>> GetAllCountries()
        {

            // CONNECT TO COUNTRY SERVICE
            var countries = await _countryService.GetAllCountries();

            // MAPPING
            var countriesResource = _mapper.Map<IEnumerable<Country>, IEnumerable<CountryResource>>(countries);
            //RETURN RESPONSE
            return Ok(countriesResource);
        }
    }
}
