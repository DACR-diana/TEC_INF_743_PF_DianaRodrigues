using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;

namespace Biblioteca.Data.Repositories.Checkouts
{
    public class FactoryRepository : Repository<FactoryRepository>
    {

        //private string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=library_1;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        private ApiDbContext ApiDbContext
        {
            get { return Context as ApiDbContext; }
        }

        public FactoryRepository(ApiDbContext context) : base(context)
        { }

        public SqlParameter AddSqlParameter(string value)
        {
            SqlParameter parameter = new SqlParameter();

            parameter = new SqlParameter("@Description", SqlDbType.VarChar)
            {
                IsNullable = true,
                Direction = ParameterDirection.Output,
                Value = value
            };
            return parameter;
        }

        public DataTable SelectQuery(string query, string[] keys, string[] values)
        {
            DataTable tabela = new DataTable();
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand command;

            connection.Open();
            command = new SqlCommand(query, connection);


            for (int i = 0; i < keys.Length; i++)
                command.Parameters.AddWithValue(keys[i], values[i]);

            SqlDataAdapter ad = new SqlDataAdapter(command);
            ad.Fill(tabela);
            connection.Close();
            return tabela;

        }
        public DataTable SelectQuery(string query)
        {
            DataTable tabela = new DataTable();
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand(query, connection);

            connection.Open();

            SqlDataAdapter ad = new SqlDataAdapter(command);
            ad.Fill(tabela);
            connection.Close();
            return tabela;
        }
        public void InsertQuery(string query, List<string> keys, List<string> values)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            SqlCommand command = new SqlCommand(query, connection);
            for (int i = 0; i < keys.Count; i++)
                command.Parameters.AddWithValue(keys[i], values[i]);
            command.ExecuteNonQuery();
            connection.Close();
        }

        public void InsertQuery(string query)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            SqlCommand command = new SqlCommand(query, connection);
            command.ExecuteNonQuery();
            connection.Close();
        }
    }
}
