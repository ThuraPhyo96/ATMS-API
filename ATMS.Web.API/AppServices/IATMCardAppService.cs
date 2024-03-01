using System.Threading.Tasks;
using ATMS.Web.Dto.Dtos;

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
