using ATMS.Web.Dto.Dtos;
using ATMS.Web.Dto.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATMS.ConsoleApp.EFCoreExamples
{
    public class EFCoreExample
    {
        private readonly AppDBContext _appDBContext;

        public EFCoreExample()
        {
            _appDBContext = new AppDBContext();
        }

        public BankCard CardLogin()
        {
            // Reqeust card number and pin from user
            Console.WriteLine("Enter card number:");
            string? cardNumber = Console.ReadLine();

            Console.WriteLine("Enter password:");
            string? password = Console.ReadLine();

            if (string.IsNullOrEmpty(cardNumber) || string.IsNullOrEmpty(password))
                return new BankCard();

            BankCard? bankCard = _appDBContext.BankCards.FirstOrDefault(x => x.BankCardNumber == cardNumber && x.PIN == password);

            if (bankCard is null)
                return new BankCard();
            return bankCard;
        }

        public BankAccountDto UpdateBalance(string cardNumber, string pin, string actionMode)
        {
            BankCard? existingBankCard = _appDBContext.BankCards
                .AsNoTracking()
                .Include(x => x.BankAccount)
                .FirstOrDefault(x => x.BankCardNumber == cardNumber && x.PIN == pin);

            if (existingBankCard is null)
                return new BankAccountDto(404, "Invalid card.", cardNumber, 0);

            Console.WriteLine("Please enter the amount.....");
            decimal amount = Convert.ToDecimal(Console.ReadLine());

            decimal availableBalance = existingBankCard.BankAccount.Balance, updatedAmount = 0m;
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

            BankAccount updatedBankAccount = new()
            {
                BankAccountId = existingBankCard.BankAccountId,
                BankAccountGuid = existingBankCard.BankAccount.BankAccountGuid,
                CustomerId = existingBankCard.CustomerId,
                AccountNumber = existingBankCard.BankAccount.AccountNumber,
                AccountType = existingBankCard.BankAccount.AccountType,
                IsActive = existingBankCard.BankAccount.IsActive,
                Balance = updatedAmount
            };

            _appDBContext.BankAccounts.Update(updatedBankAccount);
            int effectRows = _appDBContext.SaveChanges();

            if (effectRows == 0)
                return new BankAccountDto(500, "Update failed.", cardNumber, 0);
            else
            {
                BalanceHistory balanceHistory = new()
                {
                    BalanceHistoryGuid = Guid.NewGuid(),
                    CustomerId = existingBankCard.CustomerId,
                    BankAccountId = existingBankCard.BankAccountId,
                    TransactionDate = DateTime.Now,
                    Amount = amount,
                    HistoryType = historyType
                };

                _appDBContext.Add(balanceHistory);
                int history = _appDBContext.SaveChanges();

                if (history == 0)
                    return new BankAccountDto(500, "Create history failed.", cardNumber, 0);

                BankAccount? updateBankAccountObj = _appDBContext.BankAccounts.AsNoTracking().FirstOrDefault(x => x.BankAccountId == existingBankCard.BankAccountId);

                BankAccountDto bankAccount = new()
                {
                    StatusCode = 200,
                    StatusMessage = "OK",
                    AvailableBalance = updateBankAccountObj is not null ? updateBankAccountObj.Balance : 0,
                };

                return bankAccount;
            }
        }

        public List<BalanceHistory> GetHistoryByCard(string cardNumber, string pin)
        {
            // Check the card credential
            BankCard? bankCardDto = _appDBContext.BankCards.AsNoTracking().FirstOrDefault(x => x.BankCardNumber == cardNumber && x.PIN == pin);

            if (bankCardDto is null)
                return new List<BalanceHistory>();

            List<BalanceHistory>? balanceHistories = _appDBContext.BalanceHistories.AsNoTracking()
                 .Where(x => x.BankAccountId == bankCardDto.BankAccountId)
                 .ToList();

            if (!balanceHistories.Any())
                return new List<BalanceHistory>();

            return balanceHistories;
        }
    }
}
