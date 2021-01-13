using Microsoft.EntityFrameworkCore.Migrations;

namespace Biblioteca.Data.Migrations
{
    public partial class PopulateMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            #region Categories

            migrationBuilder.Sql("INSERT INTO Categories (Name) Values ('Ensaios')");
            migrationBuilder.Sql("INSERT INTO Categories (Name) Values ('Sociologia')");
            migrationBuilder.Sql("INSERT INTO Categories (Name) Values ('Romance')");
            migrationBuilder.Sql("INSERT INTO Categories (Name) Values ('Terror')");
            migrationBuilder.Sql("INSERT INTO Categories (Name) Values ('Conto')");
            migrationBuilder.Sql("INSERT INTO Categories (Name) Values ('Fantasia')");
            migrationBuilder.Sql("INSERT INTO Categories (Name) Values ('Biografia')");

            #endregion

            #region Authors

            migrationBuilder.Sql("INSERT INTO Authors (Name) Values ('Miguel de Souza Tavares')");
            migrationBuilder.Sql("INSERT INTO Authors (Name) Values ('Margarida Rebelo Pinto')");
            migrationBuilder.Sql("INSERT INTO Authors (Name) Values ('José Saramago')");
            migrationBuilder.Sql("INSERT INTO Authors (Name) Values ('António Lobo Antunes')");
            migrationBuilder.Sql("INSERT INTO Authors (Name) Values ('José Luís Peixoto')");
            migrationBuilder.Sql("INSERT INTO Authors (Name) Values ('Eça de Queirós')");
            migrationBuilder.Sql("INSERT INTO Authors (Name) Values ('Fernando Pessoa')");
            migrationBuilder.Sql("INSERT INTO Authors (Name) Values ('Pedro Chagas Freitas')");
            migrationBuilder.Sql("INSERT INTO Authors (Name) Values ('Luís de Camões')");
            migrationBuilder.Sql("INSERT INTO Authors (Name) Values ('Lídia Jorge')");

            #endregion

            #region Countries

            migrationBuilder.Sql("INSERT INTO Countries (Name) Values ('Portugal')");

            #endregion

            #region Books

            migrationBuilder.Sql("INSERT INTO Books (ISBN,Title,CountryId,State) Values ('564','Primeiro Livro',1,1)");
            migrationBuilder.Sql("INSERT INTO Books (ISBN,Title,CountryId,State) Values ('574','Segundo Livro',1,1)");

            #endregion

            #region BookAuthors

            migrationBuilder.Sql("INSERT INTO BookAuthors (BookId,AuthorId) Values (1,2)");
            migrationBuilder.Sql("INSERT INTO BookAuthors (BookId,AuthorId) Values (2,3)");

            #endregion

            #region BookCategories

            migrationBuilder.Sql("INSERT INTO BookCategories (BookId,CategoryId) Values (1,2)");
            migrationBuilder.Sql("INSERT INTO BookCategories (BookId,CategoryId) Values (2,3)");

            #endregion

            #region Clients

            migrationBuilder.Sql("INSERT INTO Clients (Registration,Name,NIF,Email,State) Values ('2020/12/12','Sandra Silva','123456789','sandra@biblioteca.com',1)");

            #endregion

            #region Employees

            migrationBuilder.Sql("INSERT INTO Employees (EmployeeNumber,Name,NIF,Email,State) Values ('36251','Admin','123457446','admin@biblioteca.com',1)");

            #endregion

            #region Checkouts

            migrationBuilder.Sql("INSERT INTO Checkouts (Date,ExpectedDate,ClientId) Values ('2020/12/21','2020/12/28',1)");

            #endregion

            #region CheckoutBooks

            migrationBuilder.Sql("INSERT INTO CheckoutBooks (BookId,CheckoutId) Values (1,1)");
            migrationBuilder.Sql("INSERT INTO CheckoutBooks (BookId,CheckoutId) Values (2,1)");


            #endregion

            #region Payments

            migrationBuilder.Sql("INSERT INTO Payments (Name) Values ('Multibanco')");


            #endregion


            #region Stored Procedure

            string storedProcedure = @"CREATE PROCEDURE [GetClientsCount]
                AS
                BEGIN
                    select count(Id) as 'Clients' from Clients 
                END";

            migrationBuilder.Sql(storedProcedure);



            #endregion
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
