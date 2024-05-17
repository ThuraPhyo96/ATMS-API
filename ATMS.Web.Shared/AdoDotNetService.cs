using System.Data;
using System.Data.SqlClient;
using Newtonsoft.Json;

namespace ATMS.Web.Shared
{
    public class AdoDotNetService
    {
        private readonly SqlConnectionStringBuilder _sqlConnectionStringBuilder;

        public AdoDotNetService(string connectionString)
        {
            _sqlConnectionStringBuilder = new SqlConnectionStringBuilder(connectionString);
        }

        public List<T> Query<T>(string query, Dictionary<string, object>? parameters = null)
        {
            SqlConnection connection = new(_sqlConnectionStringBuilder.ConnectionString);
            try
            {
                connection.Open();

                SqlCommand command = new(query, connection);
                if (parameters is not null)
                {
                    foreach (var kvp in parameters)
                    {
                        command.Parameters.AddWithValue(kvp.Key, kvp.Value);
                    }
                }

                // Add command to adapter
                SqlDataAdapter sqldataAdapter = new(command);

                // Only take one data table
                DataTable dt = new();
                sqldataAdapter.Fill(dt);

                var sObjs = JsonConvert.SerializeObject(dt);
                var records = JsonConvert.DeserializeObject<List<T>>(sObjs);
                return records!;
            }
            finally
            {
                connection.Close();
            }
        }

        public int Execute(string query, Dictionary<string, object>? parameters = null)
        {
            SqlConnection connection = new(_sqlConnectionStringBuilder.ConnectionString);
            try
            {
                connection.Open();
                SqlCommand command = new(query, connection);
                if (parameters is not null)
                {
                    foreach (var kvp in parameters)
                    {
                        command.Parameters.AddWithValue(kvp.Key, kvp.Value);
                    }
                }
                return command.ExecuteNonQuery();
            }
            finally
            {
                connection.Close();
            }
        }
    }
}
