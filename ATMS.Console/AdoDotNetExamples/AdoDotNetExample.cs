using ATMS.Web.Dto.Dtos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATMS.ConsoleApp.AdoDotNetExamples
{
    public class AdoDotNetExample
    {
        // build connection builder
        private readonly SqlConnectionStringBuilder builder = new()
        {
            DataSource = "DESKTOP-KGKMSQ5",
            InitialCatalog = "ATM_API_DB",
            UserID = "sa",
            Password = "Mingalar1"
        };
        private readonly SqlConnection _connection = new();

        public AdoDotNetExample()
        {
            // Create connection string
            _connection = new(builder.ConnectionString);
        }

        public BankCardDto CardLogin()
        {
            try
            {
                _connection.Open();
                // Reqeust card number and pin from user
                Console.WriteLine("Enter card number:");
                string? cardNumber = Console.ReadLine();

                Console.WriteLine("Enter password:");
                string? password = Console.ReadLine();

                if (string.IsNullOrEmpty(cardNumber) || string.IsNullOrEmpty(password))
                    return new BankCardDto();

                // Create raw query
                string query = @"SELECT [BankCardId]
                          ,[BankCardGuid]
                          ,[CustomerId]
                          ,[BankAccountId]
                          ,[BankCardNumber]
                          ,[PIN]
                          ,[ValidDate]
                      FROM [dbo].[BankCards]
                      WHERE BankCardNumber = @BankCardNumber AND PIN = @PIN";

                // Create command 
                SqlCommand command = new(query, _connection);

                // Add parameters
                command.Parameters.AddWithValue("@BankCardNumber", cardNumber);
                command.Parameters.AddWithValue("@PIN", password);

                // Add command to adapter
                SqlDataAdapter sqldataAdapter = new(command);

                // Only take one data table
                DataTable dt = new();
                sqldataAdapter.Fill(dt);

                if (dt.Rows.Count == 0)
                    return new BankCardDto();

                DataRow dr = dt.Rows[0];
                BankCardDto bankCard = new()
                {
                    BankCardId = Convert.ToInt16(dr["BankCardId"]),
                    BankCardNumber = dr["BankCardNumber"].ToString(),
                    PIN = dr["PIN"].ToString(),
                    ValidDate = Convert.ToDateTime(dr["ValidDate"])
                };

                return bankCard;
            }
            finally { 
                _connection.Close(); 
            }
        }
    }
}
