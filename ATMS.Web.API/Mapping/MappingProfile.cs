using ATMS.Web.Dto.Dtos;
using ATMS.Web.Dto.Models;
using AutoMapper;

namespace ATMS.Web.API.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            #region Map To Object
            CreateMap<BankAccountDto, BankAccount>();
            CreateMap<BankCardDto, BankCard>();
            CreateMap<BalanceHistoryDto, BalanceHistory>();
            CreateMap<CreateBalanceHistoryDto, BalanceHistory>();
            #endregion

            #region Map To Dto
            CreateMap<BankAccount, BankAccountDto>();
            CreateMap<BankCard, BankCardDto>();
            CreateMap<BalanceHistory, BalanceHistoryDto>();
            #endregion
        }
    }
}
