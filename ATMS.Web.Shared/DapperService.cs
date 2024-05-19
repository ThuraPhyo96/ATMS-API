using Dapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATMS.Web.Shared
{
    public class DapperService
    {
        private readonly SqlConnectionStringBuilder _sqlConnectionStringBuilder;

        public DapperService(string connectionString)
        {
            _sqlConnectionStringBuilder = new SqlConnectionStringBuilder(connectionString);
        }

        public List<T> Query<T>(string query, object? parameter = null)
        {
            IDbConnection connection = new SqlConnection(_sqlConnectionStringBuilder.ConnectionString);
            try
            {
                connection.Open();

                List<T> objs = connection.Query<T>(query, parameter).ToList();
                return objs;
            }
            finally
            {
                connection.Close();
            }
        }

        public T QueryFirstOrDefault<T>(string query, object? parameter = null)
        {
            IDbConnection connection = new SqlConnection(_sqlConnectionStringBuilder.ConnectionString);
            try
            {
                connection.Open();

                T obj = connection.QueryFirstOrDefault<T>(query, parameter)!;
                return obj;
            }
            finally
            {
                connection.Close();
            }
        }

        public int Execute(string query, object? parameter = null)
        {
            IDbConnection connection = new SqlConnection(_sqlConnectionStringBuilder.ConnectionString);
            try
            {
                connection.Open();
                int effectedRow = connection.Execute(query, parameter);
                return effectedRow;
            }
            finally
            {
                connection.Close();
            }
        }
    }
}
