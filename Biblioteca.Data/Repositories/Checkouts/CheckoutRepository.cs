using Biblioteca.Core.Models.Books;
using Biblioteca.Core.Models.Checkouts;
using Biblioteca.Core.Models.Users;
using Biblioteca.Core.Repositories.Checkouts;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.Data.Repositories.Checkouts
{
    public class CheckoutRepository : Repository<Checkout>, ICheckoutRepository
    {
        private ApiDbContext ApiDbContext
        {
            get { return Context as ApiDbContext; }
        }


        public CheckoutRepository(ApiDbContext context)
            : base(context)
        { }



        public async Task<IEnumerable<Checkout>> GetAllWithClientsAndBookAsync()
        {

            //var list = GetCheckoutBookAsync();

            //var result = ApiDbContext.Checkouts
            //    .FromSqlRaw(@"SELECT Checkouts.Id as 'Id',Clients.Id as 'ClientId',Clients.Name,Checkouts.Date,
            //                    Checkouts.DeliveryDate,Checkouts.ExpectedDate,
            //                    Books.Id as 'BookId', Books.Title FROM Checkouts
            //                    inner join Clients on Clients.Id = Checkouts.ClientId
            //                    inner join CheckoutBooks on CheckoutBooks.CheckoutId = Checkouts.Id
            //                    inner join Books on Books.Id = CheckoutBooks.BookId")
            //    .ToList();

            //var newResult = result.Join(list);


            return await ApiDbContext.Checkouts
                .(@"SELECT Checkouts.Id as 'Id',Clients.Id as 'ClientId',Clients.Name,Checkouts.Date,
                                Checkouts.DeliveryDate,Checkouts.ExpectedDate,
                                Books.Id as 'BookId', Books.Title FROM Checkouts
                                inner join Clients on Clients.Id = Checkouts.ClientId
                                inner join CheckoutBooks on CheckoutBooks.CheckoutId = Checkouts.Id
                                inner join Books on Books.Id = CheckoutBooks.BookId")
                .ToListAsync();
        }

        private List<CheckoutBook> GetCheckoutBookAsync()
        {
            return ApiDbContext.CheckoutBooks
               .FromSqlRaw(@$"SELECT Checkouts.Id as 'CheckoutId',Clients.Id as 'ClientId',Clients.Name,Checkouts.Date,
                                Checkouts.DeliveryDate,Checkouts.ExpectedDate,
                                Books.Id as 'BookId', Books.Title as 'Title' FROM Checkouts
                                inner join Clients on Clients.Id = Checkouts.ClientId
                                inner join CheckoutBooks on CheckoutBooks.CheckoutId = Checkouts.Id
                                inner join Books on Books.Id = CheckoutBooks.BookId")
                .ToList();
        }

        public async Task<IEnumerable<Checkout>> GetWithUserAndBookByIdAsync(int id)
        {
            return await ApiDbContext.Checkouts
               .FromSqlRaw(@$"SELECT Clients.Id as 'ClientId',Clients.Name,Checkouts.Date,
                                Checkouts.DeliveryDate,Checkouts.ExpectedDate,
                                Books.Id as 'BookId', Books.Title FROM Checkouts
                                inner join Clients on Clients.Id = Checkouts.ClientId
                                inner join CheckoutBooks on CheckoutBooks.CheckoutId = Checkouts.Id
                                inner join Books on Books.Id = CheckoutBooks.BookId")
               .Where(m => m.Client.Id == id)
                .ToListAsync();
        }

        public async Task<IEnumerable<Checkout>> GetAllWithUserAndBookByUserIdAsync(int userId)
        {
            return await ApiDbContext.Checkouts
               .FromSqlRaw(@"SELECT Checkouts.Id as 'CheckoutId',Clients.Id as 'ClientId',Clients.Name,Checkouts.Date,
                                Checkouts.DeliveryDate,Checkouts.ExpectedDate,
                                Books.Id as 'BookId', Books.Title FROM Checkouts
                                inner join Clients on Clients.Id = Checkouts.ClientId
                                inner join CheckoutBooks on CheckoutBooks.CheckoutId = Checkouts.Id
                                inner join Books on Books.Id = CheckoutBooks.BookId")
               .Where(m => m.Client.Id == userId)
               .ToListAsync();
        }

        public async Task<Checkout> CreateCheckout(Checkout newCheckout)
        {
            await ApiDbContext.Database.EnsureCreatedAsync();

            SqlParameter date = new SqlParameter("@Date", newCheckout.Date.ToString("s"));
            SqlParameter expectedDate = new SqlParameter("@ExpectedDate", newCheckout.ExpectedDate.ToString("s"));
            SqlParameter clientId = new SqlParameter("@ClientId", newCheckout.ClientId.ToString());

            await ApiDbContext.Database.ExecuteSqlRawAsync("INSERT INTO Checkouts (Date,ExpectedDate,ClientId) Values (@Date,@ExpectedDate,@ClientId)",
                date, expectedDate, clientId);

            ApiDbContext.SaveChanges();

            await CreateCheckoutBooks(newCheckout.Id, newCheckout.CheckoutBooks);

            return newCheckout;
        }


        private async Task<IEnumerable<Checkout>> CreateCheckoutBooks(int checkoutId, ICollection<CheckoutBook> books)
        {

            foreach (var book in books)
            {
                await ApiDbContext.Database.EnsureCreatedAsync();

                SqlParameter bookId = new SqlParameter("@BookId", book.BookId);
                SqlParameter checkoutID = new SqlParameter("@CheckoutId", checkoutId);

                await ApiDbContext.Database.ExecuteSqlRawAsync("INSERT INTO CheckoutBooks (BookId,CheckoutId) Values (@BookId,@CheckoutId)",
                    bookId, checkoutID);

                ApiDbContext.SaveChanges();
            }

            return await GetWithUserAndBookByIdAsync(checkoutId);
        }

        public async Task<Checkout> UpdateCheckout(Checkout checkout)
        {
            await ApiDbContext.Database.EnsureCreatedAsync();

            SqlParameter id = new SqlParameter("@Id", checkout.Id.ToString());
            SqlParameter deliveryDate = new SqlParameter("@DeliveryDate", checkout.DeliveryDate.ToString());

            await ApiDbContext.Database.ExecuteSqlRawAsync("UPDATE Checkouts set DeliveryDate=@Date where Id=@Id", deliveryDate, id);

            ApiDbContext.SaveChanges();

            return checkout;
        }
    }
}
