// See https://aka.ms/new-console-template for more information
using ATMS.ConsoleApp.AdoDotNetExamples;
using ATMS.ConsoleApp.DapperExamples;
using ATMS.Web.Dto.Dtos;
using ATMS.Web.Dto.Models;

internal class Program
{
    private static void Main(string[] args)
    {
        #region ADO.Net Example 
        //AdoDotNetExample adoDotNetExample = new();
        //BankCardDto bankCard = adoDotNetExample.CardLogin();

        //if (bankCard.BankCardId == 0)
        //    Console.WriteLine("Invalid card.");
        //else
        //{
        //    Console.WriteLine("Your card is valid till to " + bankCard.ValidDate.ToString("dd-MMM-yyyy"));

        //    // Ask the user to choose an option.
        //    Console.WriteLine("Choose an option from the following list:");
        //    Console.WriteLine("\tw - Withdraw");
        //    Console.WriteLine("\td - Deposit");
        //    Console.WriteLine("\th - View History");
        //    Console.Write("Your option? ");

        //    // Use a switch statement to do the math.
        //    switch (Console.ReadLine())
        //    {
        //        case "w":
        //            {
        //                BankAccountDto bankAccount = adoDotNetExample.UpdateBalance(bankCard.BankCardNumber, bankCard.PIN, "w");
        //                if (bankAccount.StatusCode == 200)
        //                {
        //                    Console.WriteLine($"Your available balance: " + bankAccount.AvailableBalance);
        //                }
        //                else
        //                {
        //                    Console.WriteLine($"Message: " + bankAccount.StatusMessage);
        //                }
        //            }
        //            break;
        //        case "d":
        //            {
        //                BankAccountDto bankAccount = adoDotNetExample.UpdateBalance(bankCard.BankCardNumber, bankCard.PIN, "d");
        //                if (bankAccount.StatusCode == 200)
        //                {
        //                    Console.WriteLine($"Your available balance: " + bankAccount.AvailableBalance);
        //                }
        //                else
        //                {
        //                    Console.WriteLine($"Message: " + bankAccount.StatusMessage);
        //                }
        //            }
        //            break;
        //        case "h":
        //            {
        //                List<BalanceHistoryDto> balanceHistories = adoDotNetExample.GetHistoryByCard(bankCard.BankCardNumber, bankCard.PIN);
        //                if (balanceHistories.Any())
        //                {
        //                    foreach (BalanceHistoryDto history in balanceHistories)
        //                    {
        //                        string historyType = string.Empty;
        //                        if (history.HistoryType == (int)EBalanceHistoryType.Withdraw)
        //                        {
        //                            historyType = "Withdraw";
        //                        }
        //                        else if (history.HistoryType == (int)EBalanceHistoryType.Deposite)
        //                        {
        //                            historyType = "Deposite";
        //                        }
        //                        Console.WriteLine($"{historyType} {history.Amount} at {history.TransactionDate: dd-MMM-yyyy}");
        //                    }
        //                }
        //                else
        //                    Console.WriteLine($"No history found.");
        //            }
        //            break;
        //    }
        //    // Wait for the user to respond before closing.
        //    Console.Write("Press any key to close the console app...");
        //    Console.ReadKey();
        //}
        #endregion

        #region Dapper Example
        DapperExample dapperExample = new();
        BankCardDto bankCard = dapperExample.CardLogin();

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
                        BankAccountDto bankAccount = dapperExample.UpdateBalance(bankCard.BankCardNumber, bankCard.PIN, "w");
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
                        BankAccountDto bankAccount = dapperExample.UpdateBalance(bankCard.BankCardNumber, bankCard.PIN, "d");
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
                        List<BalanceHistoryDto> balanceHistories = dapperExample.GetHistoryByCard(bankCard.BankCardNumber, bankCard.PIN);
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
        #endregion
    }
}