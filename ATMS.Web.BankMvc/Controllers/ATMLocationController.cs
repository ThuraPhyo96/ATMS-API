using ATM.Web.Helpers;
using ATM.Web.ViewModels;
using ATMS.Web.Dto.Dtos;
using ATMS.Web.Dto.Models;
using ATMS.Web.Shared;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Net.Mime;
using System.Text.Json;

namespace ATMS.Web.BankMvc.Controllers
{
    public class ATMLocationController : Controller
    {
        private readonly AdoDotNetService _adoDotNetService;
        private readonly JsonSerializerOptions _jsonOption;

        public ATMLocationController(AdoDotNetService adoDotNetService)
        {
            _adoDotNetService = adoDotNetService;

            // Configure serialization options
            _jsonOption = new()
            {
                PropertyNamingPolicy = null // Disable any naming policy
            };
        }

        [ActionName("Index")]
        public IActionResult ATMLocationIndex()
        {
            string query = @"SELECT [ATMLocationId]
                          ,[Address]
                          ,[Status]
                      FROM [dbo].[ATMLocations]";
            var dtos = _adoDotNetService.Query<ATMLocationDto>(query);
            var model = dtos.Select(x => ChangeToViewModel(x)).ToList();
            return View("ATMLocationIndex", model);
        }

        [HttpGet]
        [ActionName("Create")]
        public IActionResult CreateATMLocation()
        {
            CreateATMLocationViewModel model = new()
            {
                BankNames = BankNameSelectItems(),
                RegionNames = RegionSelectItems(),
                StatusNames = BankStatusSelectItems()
            };
            return View("CreateATMLocation", model);
        }

        [HttpPost]
        [ActionName("Save")]
        public IActionResult SaveATMLocation(CreateATMLocationViewModel model)
        {
            (string query, Dictionary<string, object> parameters) = GetCreateQueryAndParameters(model);
            var effectRow = _adoDotNetService.Execute(query, parameters);

            ResponseModel response = new()
            {
                IsSuccess = effectRow > 0,
                Message = effectRow > 0 ? "ATM location has been successfully created" : "Error: Creation ATM location failed!"
            };

            // Serialize your data using the specified options
            var data = JsonSerializer.Serialize(response, _jsonOption);

            return Content(data, MediaTypeNames.Application.Json);
        }

        #region Get Dropdowm items
        private List<SelectListItem> BankNameSelectItems()
        {
            string query = @"SELECT * FROM [dbo].[BankNames]";
            var dtos = _adoDotNetService.Query<BankNameDto>(query);
            return dtos.Select(x => new SelectListItem() { Value = x.BankNameId.ToString(), Text = x.Name }).ToList();
        }

        private List<SelectListItem> RegionSelectItems()
        {
            string query = @"SELECT * FROM [dbo].[Regions]";
            var dtos = _adoDotNetService.Query<RegionDto>(query);
            return dtos.Select(x => new SelectListItem() { Value = x.RegionId.ToString(), Text = x.Name }).ToList();
        }

        private List<SelectListItem> DivisionSelectItems()
        {
            string query = @"SELECT * FROM [dbo].[Divisions]";
            var dtos = _adoDotNetService.Query<DivisionDto>(query);
            return dtos.Select(x => new SelectListItem() { Value = x.DivisionId.ToString(), Text = x.Name }).ToList();
        }

        private List<SelectListItem> TownshipSelectItems()
        {
            string query = @"SELECT * FROM [dbo].[Townships]";
            var dtos = _adoDotNetService.Query<TownshipDto>(query);
            return dtos.Select(x => new SelectListItem() { Value = x.TownshipId.ToString(), Text = x.Name }).ToList();
        }

        [HttpPost]
        public IActionResult BankBranchSelectItemsByBankNameId(int bankNameId)
        {
            string query = @"SELECT * FROM [dbo].[BankBranchNames] WHERE BankNameId = @BankNameId";
            Dictionary<string, object>? parameters = new()
            {
                { "@BankNameId",bankNameId.ToString() }
            };
            var dtos = _adoDotNetService.Query<BankBranchNameDto>(query, parameters);
            List<SelectListItem> bankbranches = dtos.Select(x => new SelectListItem() { Value = x.BankBranchNameId.ToString(), Text = x.Name }).ToList();

            var data = JsonSerializer.Serialize(bankbranches, _jsonOption);
            return Content(data, MediaTypeNames.Application.Json);
        }

        [HttpPost]
        public IActionResult DivisionSelectItemsByRegionId(int regionId)
        {
            string query = @"SELECT * FROM [dbo].[Divisions] WHERE RegionId = @RegionId";
            Dictionary<string, object>? parameters = new()
            {
                { "@RegionId",regionId.ToString() }
            };
            var dtos = _adoDotNetService.Query<TownshipDto>(query, parameters);
            List<SelectListItem> divisons = dtos.Select(x => new SelectListItem() { Value = x.DivisionId.ToString(), Text = x.Name }).ToList();

            var data = JsonSerializer.Serialize(divisons, _jsonOption);
            return Content(data, MediaTypeNames.Application.Json);
        }

        [HttpPost]
        public IActionResult TownshipSelectItemsByDivisionId(int divisionId)
        {
            string query = @"SELECT * FROM [dbo].[Townships] WHERE DivisionId = @DivisionId";
            Dictionary<string, object>? parameters = new()
            {
                { "@DivisionId",divisionId.ToString() }
            };
            var dtos = _adoDotNetService.Query<TownshipDto>(query, parameters);
            List<SelectListItem> townships = dtos.Select(x => new SelectListItem() { Value = x.DivisionId.ToString(), Text = x.Name }).ToList();

            var data = JsonSerializer.Serialize(townships, _jsonOption);
            return Content(data, MediaTypeNames.Application.Json);
        }

        private List<SelectListItem> BankStatusSelectItems()
        {
            return
            [
                new()
                {
                    Value = ((int)EATMStatus.Available).ToString(),
                    Text = Helper.GetEnumDescriptionByValue<EATMStatus>((int)EATMStatus.Available)
                },
                new()
                {
                    Value = ((int)EATMStatus.TemporaryCloesd).ToString(),
                    Text = Helper.GetEnumDescriptionByValue<EATMStatus>((int)EATMStatus.TemporaryCloesd)
                }
            ];
        }
        #endregion

        private static ATMLocationViewModel ChangeToViewModel(ATMLocationDto aTMLocation)
        {
            return new ATMLocationViewModel()
            {
                Address = aTMLocation.Address
            };
        }

        private static (string, Dictionary<string, object>) GetCreateQueryAndParameters(CreateATMLocationViewModel model)
        {
            string query = @"INSERT INTO [dbo].[ATMLocations]
                                               ([BankNameId]
                                               ,[BankBranchNameId]
                                               ,[RegionId]
                                               ,[DivisionId]
                                               ,[TownshipId]
                                               ,[Address]
                                               ,[Status])
                                         VALUES
                                               (@BankNameId
                                               ,@BankBranchNameId
                                               ,@RegionId
                                               ,@DivisionId
                                               ,@TownshipId
                                               ,@Address
                                               ,@Status)";


            string? bankBranch = null;
            if (model.BankBranchNameId is not null)
                bankBranch = model.BankBranchNameId.ToString();

            Dictionary<string, object> parameters = new()
            {
                { "@BankNameId", model.BankNameId.ToString() },
                { "@BankBranchNameId", bankBranch! },
                { "@RegionId", model.RegionId.ToString() },
                { "@DivisionId", model.DivisionId.ToString() },
                { "@TownshipId", model.TownshipId.ToString() },
                { "@Address", model.Address },
                { "@Status", model.Status.ToString() },
            };

            return (query, parameters);
        }
    }
}
