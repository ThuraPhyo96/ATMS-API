using ATMS.Web.API.AppServices.Dtos;
using System.Threading.Tasks;

namespace ATMS.Web.API.AppServices
{
    public interface IATMCardAppService
    {
        Task<BankAccountDto> CardLogin(CheckBankCardDto input);
        Task<BankAccountDto> UpdateBalance(UpdateBalanceByCustomerDto input);
        Task<BalanceHistoryByCardNumberDto> GetAllHistoryByCardNumber(string cardNumber);
        Task<BankCardDto> GetBankCardByCardNumber(string cardNumber);
    }
}
