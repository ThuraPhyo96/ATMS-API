using ATMS.Web.API.AppServices;
using ATMS.Web.API.AppServices.Dtos;
using Microsoft.AspNetCore.Http;
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

        [HttpPost()]
        public async Task<IActionResult> PostAsync([FromBody] CheckBankCardDto input)
        {
            BankAccountDto result = await _aTMCardAppService.CardLogin(input);
            if (result.StatusCode == StatusCodes.Status200OK)
                return Ok(result);
            else
                return Unauthorized(result.StatusMessage);
        }

        [HttpPut()]
        public async Task<IActionResult> PutAsync([FromBody] UpdateBalanceByCustomerDto input)
        {
            BankAccountDto result = await _aTMCardAppService.UpdateBalance(input);
            if (result.StatusCode == StatusCodes.Status200OK)
                return Ok(result);
            else
                return Unauthorized(result.StatusMessage);
        }

        [HttpGet("{cardNumber}/{viewHistory}")]
        public async Task<IActionResult> GetAsync(string cardNumber, bool viewHistory)
        {
            if (viewHistory)
            {
                BalanceHistoryByCardNumberDto result = await _aTMCardAppService.GetAllHistoryByCardNumber(cardNumber);

                if (result.StatusCode == StatusCodes.Status200OK)
                    return Ok(result);
                else
                    return NotFound(result.StatusMessage);
            }
            else
            {
                BankCardDto result = await _aTMCardAppService.GetBankCardByCardNumber(cardNumber);
                if (result.StatusCode == StatusCodes.Status200OK)
                    return Ok(result);
                else
                    return NotFound(result.StatusMessage);
            }
        }
    }
}
