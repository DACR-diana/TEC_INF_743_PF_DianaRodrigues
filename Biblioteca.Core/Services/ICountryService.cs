using Biblioteca.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.Core.Services
{
    public interface ICountryService
    {
        Task<IEnumerable<Country>> GetAllCountries();
    }
}
