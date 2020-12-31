using Biblioteca.Core;
using Biblioteca.Core.Models;
using Biblioteca.Core.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.Services
{
    public class CountryService : ICountryService
    {
        private readonly IUnitOfWork _unitOfWork;
        public CountryService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Country>> GetAllCountries()
        {
            return await _unitOfWork.Countries.GetAllAsync();
        }
    }
}
