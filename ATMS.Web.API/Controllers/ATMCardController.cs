using ATMS.Web.API.AppServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Collections.Generic;
using System.Threading.Tasks;
using ATMS.Web.Dto.Dtos;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using Newtonsoft.Json;
using ATMS.Web.API.Helpers;
using ATMS.Web.Dto.Models;
using System.Linq;
using ATM.Web.ViewModels;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ATMS.Web.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ATMCardController : BaseAPIController
    {
        private readonly IATMCardAppService _aTMCardAppService;
        private readonly IConfiguration _configuration;

        public ATMCardController(IATMCardAppService aTMCardAppService,
            IConfiguration configuration)
        {
            _aTMCardAppService = aTMCardAppService;
            _configuration = configuration;
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

        [HttpGet("{bankName}")]
        public async Task<IActionResult> GetATMLocationByBankName(string bankName)
        {
            string atmLocationUrl = _configuration.GetValue<string>("ApiATMUrl");

            HttpClient client = new HttpClient();
            HttpResponseMessage result = await client.GetAsync($"{atmLocationUrl}/atmlocation/{bankName}");

            if (result.IsSuccessStatusCode)
            {
                ATMLocationListDto aTMLocationList = JsonConvert.DeserializeObject<ATMLocationListDto>(await result.Content.ReadAsStringAsync());
                return Ok(aTMLocationList.Data.Select(atmLocation => ChangeToViewModel(atmLocation)).ToList());
            }
            else
            {
                return BadRequest();
            }
        }

        private ATMLocationViewModel ChangeToViewModel(ATMLocationDto aTMLocation)
        {
            return new ATMLocationViewModel()
            {
                BankName = aTMLocation.BankName?.Name,
                BankBranchName = aTMLocation.BankBranchName?.Name,
                RegionName = aTMLocation.Region?.Name,
                DivisionName = aTMLocation.Division?.Name,
                TownshipName = aTMLocation.Township?.Name,
                Address = aTMLocation.Address,
                Status = EnumHelper.GetEnumDescriptionByValue<EATMStatus>(aTMLocation.Status)
            };
        }
    }
}
