using ATMS.Web.Dto.Dtos;

namespace ATMS.Web.ATMLocationAPI.AppServices
{
    public interface IATMLocationAppService
    {
        Task<ATMLocationListDto> GetATMLocationsByBankName(string bankName);
        Task<ATMLocationListResponseDto> GetATMLocationBy(ATMLocationFilterDto input);
    }
}
