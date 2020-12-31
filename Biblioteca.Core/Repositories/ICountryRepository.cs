using Biblioteca.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.Core.Repositories
{
    public interface ICountryRepository : IRepository<Country>
    {
        //Task<IEnumerable<Country>> GetAllCountriesAsync();
    }
}
