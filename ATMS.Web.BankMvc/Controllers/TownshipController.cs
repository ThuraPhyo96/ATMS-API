using ATM.Web.ViewModels;
using ATMS.Web.Dto.Dtos;
using ATMS.Web.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net.Mime;
using System.Text.Json;

namespace ATMS.Web.BankMvc.Controllers
{
    public class TownshipController : Controller
    {
        private readonly DapperService _dapperService;
        private readonly JsonSerializerOptions _jsonOption;

        public TownshipController(DapperService dapperService)
        {
            _dapperService = dapperService;
            // Configure serialization options
            _jsonOption = new()
            {
                PropertyNamingPolicy = null // Disable any naming policy
            };
        }

        #region CRUD
        [ActionName("Index")]
        public IActionResult TownshipIndex()
        {
            string query = @"SELECT
                                township.TownshipId,
                                region.Name AS RegionName,
                                division.Name AS DivisionName,
                                township.Name,
                                township.Description
                            FROM Townships AS township
                            LEFT JOIN Regions AS region ON region.RegionId = township.RegionId                           
                            LEFT JOIN Divisions AS division ON division.DivisionId = township.DivisionId;";

            var dtos = _dapperService.Query<TownshipResponseDto>(query);
            var model = dtos.Select(x => ChangeToViewModel(x)).ToList();
            return View("TownshipIndex", model);
        }

        [HttpGet]
        [ActionName("Create")]
        public IActionResult CreateTownship()
        {
            CreateTownshipViewModel model = new()
            {
                RegionNames = RegionSelectItems()
            };
            return View("CreateTownship", model);
        }

        [HttpPost]
        [ActionName("Save")]
        public IActionResult SaveTownship(CreateTownshipViewModel model)
        {
            (string query, Dictionary<string, object> parameters) = GetCreateQueryAndParameters(model);
            var effectRow = _dapperService.Execute(query, parameters);

            ResponseModel response = new()
            {
                IsSuccess = effectRow > 0,
                Message = effectRow > 0 ? "Township has been successfully created" : "Error: Creation township failed!"
            };

            // Serialize your data using the specified options
            string data = JsonSerializer.Serialize(response, _jsonOption);

            return Content(data, MediaTypeNames.Application.Json);
        }
        #endregion

        #region Get Dropdowm items
        private List<SelectListItem> RegionSelectItems()
        {
            string query = @"SELECT * FROM [dbo].[Regions]";
            var dtos = _dapperService.Query<RegionDto>(query);
            return dtos.Select(x => new SelectListItem() { Value = x.RegionId.ToString(), Text = x.Name }).ToList();
        }

        private List<SelectListItem> DivisionSelectItems()
        {
            string query = @"SELECT * FROM [dbo].[Divisions]";
            var dtos = _dapperService.Query<DivisionDto>(query);
            return dtos.Select(x => new SelectListItem() { Value = x.DivisionId.ToString(), Text = x.Name }).ToList();
        }

        [HttpPost]
        public IActionResult DivisionSelectItemsByRegionId(int regionId)
        {
            string query = @"SELECT * FROM [dbo].[Divisions] WHERE RegionId = @RegionId";
            Dictionary<string, object>? parameters = new()
            {
                { "@RegionId",regionId.ToString() }
            };
            var dtos = _dapperService.Query<TownshipDto>(query, parameters);
            List<SelectListItem> divisons = dtos.Select(x => new SelectListItem() { Value = x.DivisionId.ToString(), Text = x.Name }).ToList();

            var data = JsonSerializer.Serialize(divisons, _jsonOption);
            return Content(data, MediaTypeNames.Application.Json);
        }
        #endregion

        #region Get Query With Parameters
        private static TownshipViewModel ChangeToViewModel(TownshipResponseDto input)
        {
            return new TownshipViewModel()
            {
                TownshipId = input.TownshipId,
                RegionName = input!.RegionName,
                DivisionName = input!.DivisionName,
                Name = input!.Name,
                Description = input?.Description
            };
        }

        private static (string, Dictionary<string, object>) GetCreateQueryAndParameters(CreateTownshipViewModel model)
        {
            string query = @"INSERT INTO [dbo].[Townships]
                                               ([RegionId]
                                               ,[DivisionId]
                                               ,[Name]
                                               ,[Description]
                                               ,[Sort]
                                               )
                                         VALUES
                                               (
                                               @RegionId
                                               ,@DivisionId
                                               ,@Name
                                               ,@Description
                                               ,@Sort
                                               )";

            Dictionary<string, object> parameters = new()
            {
                { "@RegionId", model.RegionId.ToString() },
                { "@DivisionId", model.DivisionId.ToString() },
                { "@Name", model.Name },
                { "@Description", model.Description },
                { "@Sort", model.Sort }
            };

            return (query, parameters);
        }
        #endregion
    }
}
