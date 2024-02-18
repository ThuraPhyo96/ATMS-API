using ATMS.Web.API.AppServices;
using ATMS.Web.API.AppServices.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Collections.Generic;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ATMS.Web.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ATMCardController : BaseAPIController
    {
        private readonly IATMCardAppService _aTMCardAppService;

        public ATMCardController(IATMCardAppService aTMCardAppService)
        {
            _aTMCardAppService = aTMCardAppService;
        }

        // POST api/<ATMCardController>
        [HttpPost("cardlogin")]
        public async Task<BankAccountDto> CardLogin([FromBody] CheckBankCardDto input)
        {
            return await _aTMCardAppService.CardLogin(input);
        }

        // POST api/<ATMCardController>
        [HttpPost("updatebalance")]
        public async Task<BankAccountDto> UpdateBalance([FromBody] UpdateBalanceByCustomerDto input)
        {
            return await _aTMCardAppService.UpdateBalance(input);
        }

        // POST api/<ATMCardController>
        [HttpPost("gethistorybycardnumber")]
        public async Task<BalanceHistoryByCardNumberDto> GetHistoryByCardNumber([FromBody] string input)
        {
            return await _aTMCardAppService.GetAllHistoryByCardNumber(input);
        }
    }
}
