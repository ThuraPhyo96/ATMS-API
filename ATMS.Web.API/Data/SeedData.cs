using ATMS.Web.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace ATMS.Web.API.Data
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using var context = new ATMContext(serviceProvider.GetRequiredService<DbContextOptions<ATMContext>>());
            // Look for any customer.
            if (context.Customers.Any())
            {
                return;   // DB has been seeded
            }

            Customer customer = new Customer()
            {
                FullName = "William",
                DateOfBirth = new DateTime(1990, 1, 12),
                NRIC = "S0501796C",
                FatherName = "Connor",
                MobileNumber = "09876567654",
            };

            Customer customer2 = new Customer()
            {
                FullName = "Kyle",
                DateOfBirth = new DateTime(1992, 1, 12),
                NRIC = "S0272370J",
                FatherName = "Connor",
                MobileNumber = "09786765467"
            };

            InitialCustomer(context, customer, accountNumber: "0000111122223333", cardNumber: "9999888877776666");
            InitialCustomer(context, customer2, accountNumber: "1111222233334444", cardNumber: "8888777766665555");
        }

        private static void InitialCustomer(ATMContext context, Customer customer, string accountNumber, string cardNumber)
        {
            context.Customers.Add(customer);
            context.SaveChanges();

            BankAccount bankAccount = new BankAccount()
            {
                CustomerId = customer.CustomerId,
                AccountNumber = accountNumber,
                AccountType = (int)EBankAccountType.Saving,
                Balance = 100000M,
                IsActive = true,
            };

            context.BankAccounts.Add(bankAccount);
            context.SaveChanges();

            BankCard bankCard = new BankCard()
            {
                CustomerId = customer.CustomerId,
                BankAccountId = bankAccount.BankAccountId,
                BankCardNumber = cardNumber,
                PIN = "123456",
                ValidDate = new DateTime(2030, 12, 31)
            };
            context.BankCards.Add(bankCard);
            context.SaveChanges();
        }
    }
}
