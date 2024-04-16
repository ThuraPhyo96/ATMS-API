using ATMS.Web.Dto;
using ATMS.Web.Dto.Dtos;
using ATMS.Web.Dto.Models;
using AutoMapper;

namespace ATMS.Web.ATMLocationAPI.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            #region Map To Dto
            CreateMap<Region, RegionDto>();
            CreateMap<Division, DivisionDto>();
            CreateMap<Township, TownshipDto>();

            CreateMap<BankName, BankNameDto>();
            CreateMap<BankBranchName, BankBranchNameDto>();
            CreateMap<ATMLocation, ATMLocationDto>();
            #endregion

            #region Map To Entity
            CreateMap<RegionDto, Region>();
            CreateMap<DivisionDto, Division>();
            CreateMap<TownshipDto, Township>();

            CreateMap<BankNameDto, BankName>();
            CreateMap<BankBranchNameDto, BankBranchName>();
            CreateMap<ATMLocationDto, ATMLocation>();
            #endregion
        }
    }
}
