using ATM.Web.ViewModels;
using ATMS.Web.Dto.Dtos;
using ATMS.Web.Dto.Helpers;
using ATMS.Web.Dto.Models;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using Refit;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATMS.ConsoleApp.RefitExamples
{
    public class RefitExample
    {
        private readonly IRefitExample _refitAppService;

        public RefitExample()
        {
            _refitAppService = RestService.For<IRefitExample>("http://localhost:6420/api");
        }

        public async Task Run()
        {
            // Reqeust card number and pin from user
            Console.WriteLine("Enter card number:");
            string? cardNumber = Console.ReadLine();

            Console.WriteLine("Enter password:");
            string? password = Console.ReadLine();

            if (string.IsNullOrEmpty(cardNumber) || string.IsNullOrEmpty(password))
            {
                Console.WriteLine("Invalid card login.");
                return;
            }
            BankAccountDto bankAccount = await CardLogin(cardNumber!, password!);

            if (bankAccount.StatusCode == 0)
            {
                Console.WriteLine(bankAccount.StatusMessage);
                return;
            }

            Console.WriteLine("Your available balance: " + bankAccount.AvailableBalance);

            // Ask the user to choose an option.
            Console.WriteLine("Choose an option from the following list:");
            Console.WriteLine("\tw - Withdraw");
            Console.WriteLine("\td - Deposit");
            Console.WriteLine("\th - View History");
            Console.WriteLine("\tl - View ATM Location");
            Console.Write("Your option? ");

            // Use a switch statement to do the math.
            switch (Console.ReadLine())
            {
                case "w":
                    {
                        BankAccountDto updatedBankAccount = await UpdateBalance(cardNumber!, password!, (int)EBalanceHistoryType.Withdraw);
                        if (bankAccount.StatusCode == 200)
                        {
                            Console.WriteLine($"Your available balance: " + updatedBankAccount.AvailableBalance);
                        }
                        else
                        {
                            Console.WriteLine($"Message: " + updatedBankAccount.StatusMessage);
                        }
                    }
                    break;
                case "d":
                    {
                        BankAccountDto updatedBankAccount = await UpdateBalance(cardNumber!, password!, (int)EBalanceHistoryType.Deposite);
                        if (bankAccount.StatusCode == 200)
                        {
                            Console.WriteLine($"Your available balance: " + updatedBankAccount.AvailableBalance);
                        }
                        else
                        {
                            Console.WriteLine($"Message: " + updatedBankAccount.StatusMessage);
                        }
                    }
                    break;
                case "h":
                    {
                        BalanceHistoryByCardNumberDto balanceHistory = await GetHistoryByCard(cardNumber!);
                        if (balanceHistory.Histories.Any())
                        {
                            foreach (BalanceHistoryDto history in balanceHistory.Histories)
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
                                Console.WriteLine($"{historyType} {history.Amount: #,#00.00} at {history.TransactionDate: dd-MMM-yyyy}");
                            }
                        }
                        else
                            Console.WriteLine($"No history found.");
                    }
                    break;
                case "l":
                    {
                        Console.WriteLine("Enter bank name:");
                        string? bankName = Console.ReadLine();
                        List<ATMLocationViewModel> aTMLocations = await GetATMLocationByBankName(bankName!);
                        if (aTMLocations.Any())
                        {
                            foreach (ATMLocationViewModel aTMLocation in aTMLocations)
                            {
                                Console.WriteLine($"{aTMLocation.BankName}|{aTMLocation?.BankBranchName}|{aTMLocation?.RegionName}|{aTMLocation?.DivisionName}|{aTMLocation?.TownshipName}|{aTMLocation?.Address}|{aTMLocation?.Status}");
                            }
                        }
                        else
                            Console.WriteLine($"No ATM found.");
                    }
                    break;
            }
            // Wait for the user to respond before closing.
            Console.Write("Press any key to close the console app...");
            Console.ReadKey();
        }

        public async Task<BankAccountDto> CardLogin(string cardNumber, string password)
        {
            try
            {
                BankCard bankCard = new()
                {
                    BankCardNumber = cardNumber,
                    PIN = password
                };
                BankAccountDto? bankAccount = await _refitAppService.CheckBankCard(bankCard);
                return bankAccount;
            }
            catch (ApiException ex)
            {
                return new BankAccountDto() { StatusMessage = ex.Content?.ToString() };
            }
        }

        public async Task<BankAccountDto> UpdateBalance(string cardNumber, string pin, int actionType)
        {
            try
            {
                Console.WriteLine("Please enter the amount.....");
                decimal amount = Convert.ToDecimal(Console.ReadLine());

                UpdateBalanceByCustomerDto updateBalanceByCustomerDto = new()
                {
                    BankCardNumber = cardNumber,
                    PIN = pin,
                    ActionType = actionType,
                    Amount = amount
                };

                BankAccountDto bankAccountDto = await _refitAppService.UpdateBalanceByCustomer(updateBalanceByCustomerDto);
                return bankAccountDto;
            }
            catch (ApiException ex)
            {
                return new BankAccountDto() { StatusMessage = ex.Content?.ToString() };
            }
        }

        public async Task<BalanceHistoryByCardNumberDto> GetHistoryByCard(string cardNumber)
        {
            try
            {
                BalanceHistoryByCardNumberDto balanceHistoryByCards = await _refitAppService.BalanceHistoryByCardNumber(cardNumber, true);
                return balanceHistoryByCards;
            }
            catch (ApiException ex)
            {
                return new BalanceHistoryByCardNumberDto() { StatusMessage = ex.Content?.ToString() };
            }
        }

        public async Task<List<ATMLocationViewModel>> GetATMLocationByBankName(string bankName)
        {
            try
            {
                List<ATMLocationViewModel> locations = await _refitAppService.ATMLocationByBankName(bankName);
                return locations;
            }
            catch (ApiException ex)
            {
                Console.WriteLine($"Error calling API: {ex.Message}");
                return new List<ATMLocationViewModel>();
            }
        }
    }
}
