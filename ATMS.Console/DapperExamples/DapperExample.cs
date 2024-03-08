using ATMS.Web.Dto.Dtos;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using ATMS.Web.Dto.Models;

namespace ATMS.ConsoleApp.DapperExamples
{
    public class DapperExample
    {
        // create connection builder
        private readonly SqlConnectionStringBuilder _builder = new()
        {
            DataSource = "DESKTOP-KGKMSQ5",
            InitialCatalog = "ATM_API_DB",
            UserID = "sa",
            Password = "Mingalar1"
        };

        private readonly IDbConnection _dbConnection = new SqlConnection();

        public DapperExample()
        {
            // Create connection string
            _dbConnection.ConnectionString = _builder.ConnectionString;
        }

        public BankCardDto CardLogin()
        {
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

            BankCardDto card = new()
            {
                BankCardNumber = cardNumber,
                PIN = password
            };

            BankCardDto? bankCardDto = _dbConnection.Query<BankCardDto>(query, card).FirstOrDefault();

            if (bankCardDto is null)
                return new BankCardDto();
            return bankCardDto;
        }

        public BankAccountDto UpdateBalance(string cardNumber, string pin, string actionMode)
        {
            // Check card crednetial
            string query = @"SELECT 
                   account.[BankAccountId]
                  ,account.[Balance]
                  ,account.[CustomerId]
                  FROM [dbo].[BankCards] card
                  LEFT JOIN [dbo].[BankAccounts] account ON account.BankAccountId = card.BankAccountId
                  WHERE BankCardNumber = @BankCardNumber AND PIN = @PIN";

            BankCardDto bankCardDto = new()
            {
                BankCardNumber = cardNumber,
                PIN = pin
            };

            BankCardDto? existingBankCard = _dbConnection.Query<BankCardDto>(query, bankCardDto).FirstOrDefault();

            if (existingBankCard is null)
                return new BankAccountDto(404, "Invalid card.", cardNumber, 0);

            Console.WriteLine("Please enter the amount.....");
            decimal amount = Convert.ToDecimal(Console.ReadLine());

            int accountId = existingBankCard.BankAccountId;
            int customerId = existingBankCard.CustomerId;

            string bankAccountQuery = @"SELECT [BankAccountId]
                                          ,[Balance]
                                          FROM [dbo].[BankAccounts] 
                                          WHERE BankAccountId = @BankAccountId";

            BankAccountSingleDto bankAccountSingle = new()
            {
                BankAccountId = accountId
            };

            BankAccountSingleDto? bankAccountDto = _dbConnection.Query<BankAccountSingleDto>(bankAccountQuery, bankAccountSingle).FirstOrDefault();

            decimal availableBalance = bankAccountDto.Balance, updatedAmount = 0m;
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

            int effectRows = _dbConnection.Execute(updateQuery, new { UpdatedAmount = updatedAmount, BankAccountId = accountId });

            if (effectRows == 0)
                return new BankAccountDto(500, "Update failed.", cardNumber, 0);
            else
            {
                string effectedQuery = @"SELECT [BankAccountId]
                                          ,[Balance]
                                          FROM [dbo].[BankAccounts] 
                                          WHERE BankAccountId = @BankAccountId";

                BankAccountSingleDto? remainBalance = _dbConnection.Query<BankAccountSingleDto>(effectedQuery, bankAccountSingle).FirstOrDefault();

                if (remainBalance is null)
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

                BalanceHistoryDto balanceHistory = new()
                {
                    BalanceHistoryGuid = Guid.NewGuid(),
                    CustomerId = customerId,
                    BankAccountId = accountId,
                    TransactionDate = DateTime.Now,
                    Amount = amount,
                    HistoryType = historyType
                };

                int createdHistory = _dbConnection.Execute(createHistoryQuery, balanceHistory);

                if (createdHistory == 0)
                    return new BankAccountDto(500, "Create history failed.", cardNumber, 0);

                BankAccountDto bankAccount = new()
                {
                    StatusCode = 200,
                    StatusMessage = "OK",
                    AvailableBalance = Convert.ToDecimal(remainBalance.Balance)
                };

                return bankAccount;
            }
        }

        public List<BalanceHistoryDto> GetHistoryByCard(string cardNumber, string pin)
        {
            // Check the card credential
            string query = @"SELECT [BankAccountId]
                            FROM [dbo].[BankCards] 
                            WHERE BankCardNumber = @BankCardNumber AND
                            PIN = @PIN";

            BankCardDto bankCard = new()
            {
                BankCardNumber = cardNumber,
                PIN = pin
            };

            BankCardDto? bankCardDto = _dbConnection.Query<BankCardDto>(query, bankCard).FirstOrDefault();

            if (bankCardDto is null)
                return new List<BalanceHistoryDto>();

            int accountId = bankCardDto.BankAccountId;

            // Select all history by bank account id
            string getAllQuery = @"SELECT 
                                [TransactionDate]
                                ,[Amount]
                                ,[HistoryType]
                            FROM [dbo].[BalanceHistories] 
                            WHERE BankAccountId = @BankAccountId";

            BankAccountSingleDto bankAccountSingleDto = new()
            {
                BankAccountId = accountId
            };

            List<BalanceHistoryDto> histories = _dbConnection.Query<BalanceHistoryDto>(getAllQuery, bankAccountSingleDto).ToList();

            if (!histories.Any())
                return new List<BalanceHistoryDto>();

            return histories;
        }
    }
}
