using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Biblioteca.Core.Repositories.Checkouts
{
    public interface IFactoryRepository : IRepository<FactoryRepository>
    {
        public SqlParameter AddSqlParameter(string value);

        public DataTable SelectQuery(string query, string[] keys, string[] values);

        public DataTable SelectQuery(string query);

        public void InsertQuery(string query, List<string> keys, List<string> values);

        public void InsertQuery(string query);
    }
}
