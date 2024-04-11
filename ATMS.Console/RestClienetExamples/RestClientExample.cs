using ATMS.Web.Dto.Dtos;
using ATMS.Web.Dto.Models;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace ATMS.ConsoleApp.RestClienetExamples
{
    public class RestClientExample
    {
        private readonly string _baseUrl;

        public RestClientExample()
        {
            _baseUrl = "http://localhost:6420/api";
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
                Console.WriteLine("Invalid card login.");
                return;
            }

            Console.WriteLine("Your available balance: " + bankAccount.AvailableBalance);

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
                        BalanceHistoryByCardNumberDto balanceHistory = await GetHistoryByCard(cardNumber!, password!);
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
            }
            // Wait for the user to respond before closing.
            Console.Write("Press any key to close the console app...");
            Console.ReadKey();
        }

        public async Task<BankAccountDto> CardLogin(string cardNumber, string password)
        {
            BankCard bankCard = new()
            {
                BankCardNumber = cardNumber,
                PIN = password
            };

            try
            {
                RestRequest request = new($"{_baseUrl}/atmcard", Method.Post);
                request.AddBody(bankCard);
                RestClient client = new();
                RestResponse response = await client.ExecuteAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    BankAccountDto? bankAccount = JsonConvert.DeserializeObject<BankAccountDto>(response.Content!);
                    if (bankAccount is null)
                        return new BankAccountDto();
                    else
                        return bankAccount;
                }
                else
                {
                    return new BankAccountDto();
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error calling API: {ex.Message}");
            }

            return new BankAccountDto();
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

                RestRequest request = new($"{_baseUrl}/atmcard", Method.Post);
                request.AddBody(updateBalanceByCustomerDto);
                RestClient client = new();
                RestResponse response = await client.ExecuteAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    BankAccountDto? bankAccountDto = JsonConvert.DeserializeObject<BankAccountDto>(response.Content!);
                    if (bankAccountDto is null)
                    {
                        return new BankAccountDto() { StatusMessage = bankAccountDto?.StatusMessage };
                    }
                    else
                    {
                        return bankAccountDto;
                    }
                }
                else
                {
                    return new BankAccountDto();
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error calling API: {ex.Message}");
            }
            return new BankAccountDto();
        }

        public async Task<BalanceHistoryByCardNumberDto> GetHistoryByCard(string cardNumber, string pin)
        {
            try
            {
                RestRequest request = new($"{_baseUrl}/atmcard/{cardNumber}/true", Method.Get);
                RestClient client = new();
                RestResponse response = await client.ExecuteAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    BalanceHistoryByCardNumberDto balanceHistoryByCards = JsonConvert.DeserializeObject<BalanceHistoryByCardNumberDto>(response.Content!)!;
                    if (balanceHistoryByCards is null)
                        return new BalanceHistoryByCardNumberDto();
                    else
                        return balanceHistoryByCards;
                }
                else
                    return new BalanceHistoryByCardNumberDto();
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error calling API: {ex.Message}");
            }
            return new BalanceHistoryByCardNumberDto();
        }
    }
}
