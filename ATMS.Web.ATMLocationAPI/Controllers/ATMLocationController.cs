using ATMS.Web.ATMLocationAPI.AppServices;
using ATMS.Web.Dto.Dtos;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.Json;
using Microsoft.VisualBasic;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ATMS.Web.ATMLocationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ATMLocationController : ControllerBase
    {
        private readonly IATMLocationAppService _aTMLocationAppService;

        public ATMLocationController(IATMLocationAppService aTMLocationAppService)
        {
            _aTMLocationAppService = aTMLocationAppService;
        }

        // GET: api/<ATMLocationController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<ATMLocationController>/Kanbawza Bank
        [HttpGet("{bankName}")]
        public async Task<ActionResult> Get(string bankName)
        {
            ATMLocationListDto aTMLocation = await _aTMLocationAppService.GetATMLocationsByBankName(bankName);
            if (aTMLocation.StatusCode == StatusCodes.Status200OK)
            {
                return Ok(JsonConvert.SerializeObject(aTMLocation));
            }
            else
                return NotFound();
        }

        // POST api/<ATMLocationController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ATMLocationController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ATMLocationController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
