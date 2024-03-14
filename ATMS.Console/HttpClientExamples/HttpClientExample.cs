using ATMS.Web.Dto.Dtos;
using ATMS.Web.Dto.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;

namespace ATMS.ConsoleApp.HttpClientExamples
{
    public class HttpClientExample
    {
        private readonly HttpClient _client;
        private readonly string _baseUrl;

        public HttpClientExample()
        {
            // valid to localhost certificate
            HttpClientHandler handler = new()
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) =>
                {
                    return true;
                }
            };

            _client = new(handler)
            {
                BaseAddress = new Uri("https://localhost:44332/")
            };
            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));

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
                HttpContent content = new StringContent(JsonConvert.SerializeObject(bankCard), Encoding.UTF8, MediaTypeNames.Application.Json);
                HttpResponseMessage httpResponse = await _client.PostAsync($"{_baseUrl}/atmcard", content);

                if (httpResponse.IsSuccessStatusCode)
                {
                    BankAccountDto? bankAccount = JsonConvert.DeserializeObject<BankAccountDto>(await httpResponse.Content.ReadAsStringAsync());
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

                HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(updateBalanceByCustomerDto), Encoding.UTF8, MediaTypeNames.Application.Json);
                HttpResponseMessage httpResponse = await _client.PutAsync($"{_baseUrl}/atmcard", httpContent);

                if (httpResponse.IsSuccessStatusCode)
                {
                    BankAccountDto? bankAccountDto = JsonConvert.DeserializeObject<BankAccountDto>(await httpResponse.Content.ReadAsStringAsync());
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
                HttpResponseMessage httpResponse = await _client.GetAsync($"{_baseUrl}/atmcard/{cardNumber}/true");

                if (httpResponse.IsSuccessStatusCode)
                {
                    BalanceHistoryByCardNumberDto balanceHistoryByCards = JsonConvert.DeserializeObject<BalanceHistoryByCardNumberDto>(await httpResponse.Content.ReadAsStringAsync())!;
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
