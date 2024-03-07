// See https://aka.ms/new-console-template for more information
using ATMS.ConsoleApp.AdoDotNetExamples;
using ATMS.Web.Dto.Dtos;

internal class Program
{
    private static void Main(string[] args)
    {
        AdoDotNetExample adoDotNetExample = new();
        BankCardDto bankCard = adoDotNetExample.CardLogin();

        if (bankCard.BankCardId == 0)
            Console.WriteLine("Invalid card.");
        else
        {
            Console.WriteLine("Card nubmer: " + bankCard.BankCardNumber);
            Console.WriteLine("PIN: " + bankCard.PIN);
            Console.WriteLine("Valid date: " + bankCard.ValidDate.ToString("dd-MMM-yyyy"));
        }
    }
}