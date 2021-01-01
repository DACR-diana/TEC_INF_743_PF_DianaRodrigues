using Biblioteca.Core.Repositories;
using Biblioteca.Core.Repositories.Books;
using Biblioteca.Core.Repositories.Checkouts;
using Biblioteca.Core.Repositories.Users;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.Core
{
    public interface IUnitOfWork : IDisposable
    {
        #region Books

        IBookRepository Books { get; }
        IBookCategoryRepository BookCategories { get; }
        IBookAuthorRepository BookAuthors { get; }
        IAuthorRepository Authors { get; }
        ICategoryRepository Categories { get; }

        #endregion



        #region Users
        IClientRepository Clients { get; }
        IEmployeeRepository Employees { get; }
        #endregion

        ICountryRepository Countries { get; }

        ITicketRepository Tickets { get; }
        ICheckoutRepository Checkouts { get; }

        IPaymentRepository Payments { get; }


        Task<int> CommitAsync();
    }
}
