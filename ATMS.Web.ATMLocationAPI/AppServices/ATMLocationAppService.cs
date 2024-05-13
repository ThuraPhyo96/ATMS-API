using ATMS.Web.ATMLocationAPI.Data;
using ATMS.Web.Dto.Dtos;
using ATMS.Web.Dto.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace ATMS.Web.ATMLocationAPI.AppServices
{
    public class ATMLocationAppService : IATMLocationAppService
    {
        private readonly ATMLocationContext _context;
        private readonly IMapper _mapper;

        public ATMLocationAppService(ATMLocationContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        #region Get
        public async Task<ATMLocationListDto> GetATMLocationsByBankName(string bankName)
        {
            var query = _context.ATMLocations
                .Include(x => x.BankName)
                .Include(x => x.BankBranchName)
                .Include(x => x.Region)
                .Include(x => x.Division)
                .Include(x => x.Township)
                .AsNoTracking();

            var objs = await query
                .Where(x => x.BankName.Name.Equals(bankName))
                .ToListAsync();

            if (objs.Count != 0)
            {
                var atms = _mapper.Map<List<ATMLocationDto>>(objs);
                return new ATMLocationListDto
                {
                    Data = atms!,
                    StatusCode = StatusCodes.Status200OK,
                    StatusMessage = "success"
                };
            }
            else
            {
                return new ATMLocationListDto
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    StatusMessage = "no data found!"
                };
            }
        }

        public async Task<ATMLocationListResponseDto> GetATMLocationBy(ATMLocationFilterDto input)
        {
            var query = _context.ATMLocations
                .Include(x => x.BankName)
                .Include(x => x.BankBranchName)
                .Include(x => x.Region)
                .Include(x => x.Division)
                .Include(x => x.Township)
                .AsNoTracking();

            var total = await query.CountAsync();

            int pageCount = total / input.PageSize;
            if (total % input.PageSize > 0)
                pageCount++;

            var objs = await query
                .Skip(input.PageSize * (input.PageNumber - 1))
                .Take(input.PageSize)
                .ToListAsync();

            if (objs.Count != 0)
            {
                var atms = _mapper.Map<List<ATMLocationDto>>(objs);
                return new ATMLocationListResponseDto
                {
                    PageNumber = input.PageNumber,
                    PageSize = input.PageSize,
                    PageCount = pageCount,
                    Data = atms!,
                    StatusCode = StatusCodes.Status200OK,
                    StatusMessage = "success"
                };
            }
            else
            {
                return new ATMLocationListResponseDto
                {
                    PageNumber = input.PageNumber,
                    PageSize = input.PageSize,
                    PageCount = input.PageSize,
                    StatusCode = StatusCodes.Status404NotFound,
                    StatusMessage = "no data found!"
                };
            }
        }
        #endregion
    }
}
