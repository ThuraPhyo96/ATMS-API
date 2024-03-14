using ATMS.Web.Dto.Dtos;
using ATMS.Web.Dto.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATMS.ConsoleApp.AdoDotNetExamples
{
    public class AdoDotNetExample
    {
        // create connection builder
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

        public void Run()
        {
            BankCardDto bankCard = CardLogin();

            if (bankCard.BankCardId == 0)
                Console.WriteLine("Invalid card.");
            else
            {
                Console.WriteLine("Your card is valid till to " + bankCard.ValidDate.ToString("dd-MMM-yyyy"));

                // Ask the user to choose an option.
                Console.WriteLine("Choose an option from the following list:");
                Console.WriteLine("\tw - Withdraw");
                Console.WriteLine("\td - Deposit");
                Console.WriteLine("\th - View History");
                Console.Write("Your option? ");

                // Use a switch statement to do the math.
                switch (Console.ReadLine())
                {
                    case "w":
                        {
                            BankAccountDto bankAccount = UpdateBalance(bankCard.BankCardNumber, bankCard.PIN, "w");
                            if (bankAccount.StatusCode == 200)
                            {
                                Console.WriteLine($"Your available balance: " + bankAccount.AvailableBalance);
                            }
                            else
                            {
                                Console.WriteLine($"Message: " + bankAccount.StatusMessage);
                            }
                        }
                        break;
                    case "d":
                        {
                            BankAccountDto bankAccount = UpdateBalance(bankCard.BankCardNumber, bankCard.PIN, "d");
                            if (bankAccount.StatusCode == 200)
                            {
                                Console.WriteLine($"Your available balance: " + bankAccount.AvailableBalance);
                            }
                            else
                            {
                                Console.WriteLine($"Message: " + bankAccount.StatusMessage);
                            }
                        }
                        break;
                    case "h":
                        {
                            List<BalanceHistoryDto> balanceHistories = GetHistoryByCard(bankCard.BankCardNumber, bankCard.PIN);
                            if (balanceHistories.Any())
                            {
                                foreach (BalanceHistoryDto history in balanceHistories)
                                {
                                    string historyType = string.Empty;
                                    if (history.HistoryType == (int)EBalanceHistoryType.Withdraw)
                                    {
                                        historyType = "Withdraw";
                                    }
                                    else if (history.HistoryType == (int)EBalanceHistoryType.Deposite)
                                    {
                                        historyType = "Deposite";
                                    }
                                    Console.WriteLine($"{historyType} {history.Amount} at {history.TransactionDate: dd-MMM-yyyy}");
                                }
                            }
                            else
                                Console.WriteLine($"No history found.");
                        }
                        break;
                }
                // Wait for the user to respond before closing.
                Console.Write("Press any key to close the console app...");
                Console.ReadKey();
            }
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
            finally
            {
                _connection.Close();
            }
        }

        public BankAccountDto UpdateBalance(string cardNumber, string pin, string actionMode)
        {
            try
            {
                _connection.Open();

                // Check card crednetial
                string query = @"SELECT 
                   account.[BankAccountId]
                  ,account.[Balance]
                  ,account.[CustomerId]
                  FROM [dbo].[BankCards] card
                  LEFT JOIN [dbo].[BankAccounts] account ON account.BankAccountId = card.BankAccountId
                  WHERE BankCardNumber = @BankCardNumber AND PIN = @PIN";

                SqlCommand sqlCommand = new(query, _connection);

                sqlCommand.Parameters.AddWithValue("@BankCardNumber", cardNumber);
                sqlCommand.Parameters.AddWithValue("@PIN", pin);

                SqlDataAdapter dataAdapter = new(sqlCommand);

                DataTable dt = new();
                dataAdapter.Fill(dt);

                if (dt.Rows.Count == 0)
                    return new BankAccountDto(404, "Invalid card.", cardNumber, 0);

                Console.WriteLine("Please enter the amount.....");
                decimal amount = Convert.ToDecimal(Console.ReadLine());

                DataRow dr = dt.Rows[0];

                int accountId = Convert.ToInt16(dr["BankAccountId"]);
                decimal availableBalance = Convert.ToDecimal(dr["Balance"]);
                int customerId = Convert.ToInt16(dr["CustomerId"]);

                decimal updatedAmount = 0m;
                int historyType = 0;
                if (actionMode == "w")
                {
                    if (amount > availableBalance - 1000)
                    {
                        return new BankAccountDto(401, "You do not have enought amount.", cardNumber, 0);
                    }
                    else
                    {
                        updatedAmount = availableBalance - amount;
                        historyType = (int)EBalanceHistoryType.Withdraw;
                    }
                }
                else if (actionMode == "d")
                {
                    updatedAmount = availableBalance + amount;
                    historyType = (int)EBalanceHistoryType.Deposite;
                }

                // Update the avalilable balance 
                string updateQuery = @"UPDATE [dbo].[BankAccounts]
                           SET [Balance] = @UpdatedAmount   
                           WHERE BankAccountId = @BankAccountId";

                SqlCommand updateCommand = new(updateQuery, _connection);

                updateCommand.Parameters.AddWithValue("@UpdatedAmount", updatedAmount);
                updateCommand.Parameters.AddWithValue("@BankAccountId", accountId);

                int effectRows = updateCommand.ExecuteNonQuery();

                if (effectRows == 0)
                    return new BankAccountDto(500, "Update failed.", cardNumber, 0);
                else
                {
                    string effectedQuery = @"SELECT [Balance]
                                          FROM [dbo].[BankAccounts] 
                                          WHERE BankAccountId = @BankAccountId";

                    SqlCommand effectedCommand = new(effectedQuery, _connection);

                    effectedCommand.Parameters.AddWithValue("@BankAccountId", accountId);

                    SqlDataAdapter sqlDataAdapter = new(effectedCommand);
                    DataTable updateBankAccount = new();
                    sqlDataAdapter.Fill(updateBankAccount);

                    if (updateBankAccount.Rows.Count == 0)
                        return new BankAccountDto(500, "Read updated bank account failed", cardNumber, 0);

                    // Create the history
                    string createHistoryQuery = @"INSERT INTO [dbo].[BalanceHistories]
                                               ([BalanceHistoryGuid]
                                               ,[CustomerId]
                                               ,[BankAccountId]
                                               ,[TransactionDate]
                                               ,[Amount]
                                               ,[HistoryType])
                                               VALUES
                                               (@BalanceHistoryGuid
                                               ,@CustomerId
                                               ,@BankAccountId
                                               ,@TransactionDate
                                               ,@Amount
                                               ,@HistoryType)";

                    SqlCommand createCommand = new(createHistoryQuery, _connection);
                    createCommand.Parameters.AddWithValue("@BalanceHistoryGuid", Guid.NewGuid());
                    createCommand.Parameters.AddWithValue("@CustomerId", customerId);
                    createCommand.Parameters.AddWithValue("@BankAccountId", accountId);
                    createCommand.Parameters.AddWithValue("@TransactionDate", DateTime.Now);
                    createCommand.Parameters.AddWithValue("@Amount", amount);
                    createCommand.Parameters.AddWithValue("@HistoryType", historyType);

                    int createdHistory = createCommand.ExecuteNonQuery();

                    if (createdHistory == 0)
                        return new BankAccountDto(500, "Create history failed.", cardNumber, 0);

                    BankAccountDto bankAccount = new()
                    {
                        StatusCode = 200,
                        StatusMessage = "OK",
                        AvailableBalance = Convert.ToDecimal(updateBankAccount.Rows[0]["Balance"])
                    };

                    return bankAccount;
                }
            }
            finally
            {
                _connection.Close();
            }
        }

        public List<BalanceHistoryDto> GetHistoryByCard(string cardNumber, string pin)
        {
            try
            {
                _connection.Open();

                // Check the card credential
                string query = @"SELECT [BankAccountId]
                                FROM [dbo].[BankCards] 
                                WHERE BankCardNumber = @BankCardNumber AND
                                PIN = @PIN";

                SqlCommand queryCommand = new(query, _connection);
                queryCommand.Parameters.AddWithValue("@BankCardNumber", cardNumber);
                queryCommand.Parameters.AddWithValue("@PIN", pin);

                SqlDataAdapter sqlDataAdapter = new(queryCommand);
                DataTable updateBankAccount = new();
                sqlDataAdapter.Fill(updateBankAccount);

                if (updateBankAccount.Rows.Count == 0)
                    return new List<BalanceHistoryDto>();

                int accountId = Convert.ToInt16(updateBankAccount.Rows[0]["BankAccountId"]);

                // Select all history by bank account id
                string getAllQuery = @"SELECT 
                                           [TransactionDate]
                                          ,[Amount]
                                          ,[HistoryType]
                                      FROM [dbo].[BalanceHistories] 
                                      WHERE BankAccountId = @BankAccountId";

                SqlCommand sqlCommand = new(getAllQuery, _connection);
                sqlCommand.Parameters.AddWithValue("@BankAccountId", accountId);

                SqlDataAdapter getAllAdapter = new(sqlCommand);
                DataTable dataTable = new();

                getAllAdapter.Fill(dataTable);

                if (dataTable.Rows.Count == 0)
                    return new List<BalanceHistoryDto>();

                List<BalanceHistoryDto> histories = new();

                foreach (DataRow row in dataTable.Rows)
                {
                    BalanceHistoryDto balanceHistory = new();
                    if (!row.IsNull("Amount"))
                        balanceHistory.Amount = Convert.ToDecimal(row["Amount"]);

                    if (!row.IsNull("TransactionDate"))
                        balanceHistory.TransactionDate = Convert.ToDateTime(row["TransactionDate"]);

                    if (!row.IsNull("HistoryType"))
                        balanceHistory.HistoryType = Convert.ToInt16(row["HistoryType"]);

                    histories.Add(balanceHistory);
                }

                return histories;
            }
            finally
            {
                _connection.Close();
            }
        }
    }
}
