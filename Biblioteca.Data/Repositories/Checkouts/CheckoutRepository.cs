﻿using Biblioteca.Core.Models.Books;
using Biblioteca.Core.Models.Checkouts;
using Biblioteca.Core.Models.Users;
using Biblioteca.Core.Repositories.Checkouts;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
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

        private FactoryRepository factory = new FactoryRepository();


        public List<Checkout> GetWithCheckoutBooksByFilter(string[] filters, string[] filters_text)
        {
            HashSet<string> keys = new HashSet<string>();
            HashSet<string> values = new HashSet<string>();

            for (int i = 0; i < filters_text.Length; i++)
            {
                keys.Add($"@{filters[i].Replace(".", "")}");
                values.Add(filters_text[i].ToString());
            }


            string query = @"SELECT Checkouts.Id as 'Id',Clients.Id as 'ClientId',Clients.Name,Checkouts.Date,
                                Checkouts.DeliveryDate,Checkouts.ExpectedDate,
                                Books.Id as 'BookId', Books.Title as 'Title' FROM Checkouts
                                inner join Clients on Clients.Id = Checkouts.ClientId
                                inner join CheckoutBooks on CheckoutBooks.CheckoutId = Checkouts.Id
                               inner join Books on Books.Id = CheckoutBooks.BookId
                                 Where";


            for (int i = 0; i < filters.Length; i++)
                query += i == 0 ? $" {filters[i]}={keys.ToArray()[i]}" : $" AND {filters[i]}={keys.ToArray()[i]}";

            DataTable dataTableCheckouts = factory.SelectQuery(query, keys.ToArray(), values.ToArray());

            List<Checkout> checkouts = new List<Checkout>();
            Checkout checkout = new Checkout();
            List<CheckoutBook> checkoutBooks = new List<CheckoutBook>();

            var pastId = 0;

            if (dataTableCheckouts.Rows.Count > 0)
            {
                for (int i = 0; i < dataTableCheckouts.Rows.Count; i++)
                {
                    Client client = new Client();

                    client.Id = int.Parse(dataTableCheckouts.Rows[i]["ClientId"].ToString());
                    client.Name = dataTableCheckouts.Rows[i]["Name"].ToString();
                    checkout.Id = int.Parse(dataTableCheckouts.Rows[i]["Id"].ToString());
                    if (i == 0)
                    {
                        pastId = checkout.Id;

                        checkout.DeliveryDate = dataTableCheckouts.Rows[i]["DeliveryDate"].ToString() == string.Empty ? (DateTime?)null : DateTime.Parse(dataTableCheckouts.Rows[i]["DeliveryDate"].ToString());
                        checkout.ExpectedDate = DateTime.Parse(dataTableCheckouts.Rows[i]["ExpectedDate"].ToString());

                        CheckoutBook checkoutBook = new CheckoutBook();
                        Book book = new Book();

                        book.Id = int.Parse(dataTableCheckouts.Rows[i]["BookId"].ToString());
                        book.Title = dataTableCheckouts.Rows[i]["Title"].ToString();
                        checkoutBook.Book = book;

                        checkoutBooks.Add(checkoutBook);
                        checkout.CheckoutBooks = checkoutBooks;
                    }
                    else if (pastId== checkout.Id)
                    {

                        CheckoutBook checkoutBook = new CheckoutBook();
                        Book book = new Book();

                        book.Id = int.Parse(dataTableCheckouts.Rows[i]["BookId"].ToString());
                        book.Title = dataTableCheckouts.Rows[i]["Title"].ToString();
                        checkoutBook.Book = book;

                        checkout.CheckoutBooks.Where(b=>b.CheckoutId==checkout.Id).Add(checkoutBook);
                    }


                    checkouts.Add(checkout);

                }


            }
            return checkouts;
        }

        public List<Checkout> GetWithCheckoutBooksByFilterByState(string[] filters, string[] filters_text, bool state)
        {
            HashSet<string> keys = new HashSet<string>();
            HashSet<string> values = new HashSet<string>();

            for (int i = 0; i < filters_text.Length; i++)
            {
                keys.Add($"@{filters[i].Replace(".", "")}");
                values.Add(filters_text[i].ToString());
            }


            string query = @"SELECT Checkouts.Id as 'Id',Clients.Id as 'ClientId',Clients.Name,Checkouts.Date,
                                Checkouts.DeliveryDate,Checkouts.ExpectedDate,
                                Books.Id as 'BookId', Books.Title as 'Title' FROM Checkouts
                                inner join Clients on Clients.Id = Checkouts.ClientId
                                inner join CheckoutBooks on CheckoutBooks.CheckoutId = Checkouts.Id
                               inner join Books on Books.Id = CheckoutBooks.BookId
                                 Where";

            query += state == true ? " Checkouts.DeliveryDate is null" : "Checkouts.DeliveryDate is not null";

            for (int i = 0; i < filters.Length; i++)
                query += $" AND {filters[i]}={keys.ToArray()[i]}";

            DataTable dataTableCheckouts = factory.SelectQuery(query, keys.ToArray(), values.ToArray());

            List<Checkout> checkouts = new List<Checkout>();
            Checkout checkout = new Checkout();
            List<CheckoutBook> checkoutBooks = new List<CheckoutBook>();


            if (dataTableCheckouts.Rows.Count > 0)
            {
                Client client = new Client();

                client.Id = int.Parse(dataTableCheckouts.Rows[0]["ClientId"].ToString());
                client.Name = dataTableCheckouts.Rows[0]["Name"].ToString();

                checkout.Id = int.Parse(dataTableCheckouts.Rows[0]["Id"].ToString());
                checkout.DeliveryDate = dataTableCheckouts.Rows[0]["DeliveryDate"].ToString() == string.Empty ? (DateTime?)null : DateTime.Parse(dataTableCheckouts.Rows[0]["DeliveryDate"].ToString());
                checkout.ExpectedDate = DateTime.Parse(dataTableCheckouts.Rows[0]["ExpectedDate"].ToString());

                for (int i = 0; i < dataTableCheckouts.Rows.Count; i++)
                {
                    CheckoutBook checkoutBook = new CheckoutBook();
                    Book book = new Book();

                    book.Id = int.Parse(dataTableCheckouts.Rows[i]["BookId"].ToString());
                    book.Title = dataTableCheckouts.Rows[i]["Title"].ToString();
                    checkoutBook.Book = book;

                    checkoutBooks.Add(checkoutBook);
                }

                checkout.CheckoutBooks = checkoutBooks;
                checkouts.Add(checkout);
            }
            return checkouts;
        }
        public List<Checkout> CreateCheckout(Checkout newCheckout)
        {

            string[] keys = { "@Date", "@ExpectedDate", "ClientId" };
            string[] values = { newCheckout.Date.ToString("s"), newCheckout.ExpectedDate.ToString("s"), newCheckout.ClientId.ToString() };

            string query = @"INSERT INTO Checkouts (Date,ExpectedDate,ClientId) Output Inserted.Id Values (@Date,@ExpectedDate,@ClientId)";
            var id = factory.SelectQuery(query, keys, values).Rows[0][0].ToString();
            return CreateCheckoutBooks(int.Parse(id), newCheckout.CheckoutBooks);
        }

        private List<Checkout> CreateCheckoutBooks(int checkoutId, ICollection<CheckoutBook> books)
        {

            foreach (var book in books)
            {
                string[] keys = { "@BookId", "@CheckoutId" };
                string[] values = { book.BookId.ToString(), checkoutId.ToString() };

                string query = @"INSERT INTO CheckoutBooks (BookId,CheckoutId) Values (@BookId,@CheckoutId)";

                factory.SelectQuery(query, keys, values);
            }
            return GetWithCheckoutBooksByFilter(new string[] { "Checkouts.Id" }, new string[] { checkoutId.ToString() });
        }

        private void DeleteCheckoutBooks(int checkoutId)
        {

            string[] keys = { "@CheckoutId" };
            string[] values = { checkoutId.ToString() };

            string query = @"DELETE FROM CheckoutBooks WHERE CheckoutId=@CheckoutId";

            factory.SelectQuery(query, keys, values);
        }

        public List<Checkout> UpdateCheckout(Checkout checkout)
        {
            string[] keys = { "@Id", "@DeliveryDate" };
            string[] values = { checkout.Id.ToString(), checkout.DeliveryDate.ToString() };

            string query = @"UPDATE Checkouts set DeliveryDate=@DeliveryDate where Id=@Id";

            factory.SelectQuery(query, keys, values);
            DeleteCheckoutBooks(checkout.Id);
            CreateCheckoutBooks(checkout.Id, checkout.CheckoutBooks);

            return GetWithCheckoutBooksByFilter(new string[] { "Checkouts.Id" }, new string[] { checkout.Id.ToString() });
        }
    }
}
