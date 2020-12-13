using Biblioteca.Core.Models;
using Biblioteca.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Biblioteca.Data.Repositories
{
    public class CountryRepository : Repository<Country>, ICountryRepository
    {
        private ApiDbContext ApiDbContext
        {
            get { return Context as ApiDbContext; }
        }

        public CountryRepository(ApiDbContext context)
            : base(context)
        { }
    }
}
