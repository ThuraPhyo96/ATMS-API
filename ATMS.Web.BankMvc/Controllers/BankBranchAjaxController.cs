using ATM.Web.Helpers;
using ATM.Web.ViewModels;
using ATMS.Web.BankMvc.Data;
using ATMS.Web.Dto.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Net.Mime;
using System.Text.Json;

namespace ATMS.Web.BankMvc.Controllers
{
    public class BankBranchAjaxController : Controller
    {
        private readonly ApplicationDBContext _dBContext;
        private readonly JsonSerializerOptions _jsonOption;

        public BankBranchAjaxController()
        {
            _dBContext = new ApplicationDBContext();

            // Configure serialization options
            _jsonOption = new()
            {
                PropertyNamingPolicy = null // Disable any naming policy
            };
        }

        [ActionName("Index")]
        public async Task<IActionResult> BankBranchIndex(int pageNo = 1, int pageSize = 1)
        {
            var query = _dBContext.BankBranchNames
                .Include(x => x.BankName)
                .Include(x => x.Region)
                .Include(x => x.Division)
                .Include(x => x.Township)
                .AsNoTracking();

            var total = await query.CountAsync();
            int pageCount = total / pageSize;
            if (total % pageSize > 0)
                pageCount++;

            var objs = await query
                .Skip(pageSize * (pageNo - 1))
                .Take(pageSize)
                .ToListAsync();

            var records = objs.Select(x => ChangeBankBranchViewModel(x)).ToList();

            var model = new BankBranchListViewModel
            {
                PageNumber = pageNo,
                PageSize = pageSize,
                PageCount = pageCount,
                Data = records
            };
            return View("BankBranchIndex", model);
        }

        [HttpGet]
        [ActionName("Create")]
        public async Task<IActionResult> CreateBankBranch()
        {
            CreateBankBranchViewModel model = new()
            {
                BankNames = await BankNameSelectItems(),
                RegionNames = await RegionSelectItems(),
                StatusNames = BankStatusSelectItems(),
                OpeningHour = new TimeSpan(9, 30, 0),
                ClosedHour = new TimeSpan(15, 30, 0)
            };
            return View("CreateBankBranch", model);
        }

        [HttpPost]
        [ActionName("Save")]
        public async Task<IActionResult> SaveBankBranch(CreateBankBranchViewModel model)
        {
            var obj = ChangeBankBranchNameModel(model);
            _dBContext.BankBranchNames.Add(obj);
            int effectRow = await _dBContext.SaveChangesAsync();

            ResponseModel response = new()
            {
                IsSuccess = effectRow > 0,
                Message = effectRow > 0 ? "Bank branch has been successfully created" : "Error: Creation bank failed!"
            };

            // Serialize your data using the specified options
            var data = System.Text.Json.JsonSerializer.Serialize(response, _jsonOption);

            return Content(data, MediaTypeNames.Application.Json);
        }

        [HttpGet]
        [ActionName("Edit")]
        public async Task<IActionResult> EditBankBranch(int id)
        {
            var query = _dBContext.BankBranchNames
                 .Include(x => x.BankName)
                 .Include(x => x.Region)
                 .Include(x => x.Division)
                 .Include(x => x.Township)
                 .AsNoTracking();

            var bankObj = await query.FirstOrDefaultAsync(x => x.BankBranchNameId.Equals(id));
            if (bankObj is null)
                return BadRequest();
            else
            {
                var model = ChangeBankBranchViewModel(bankObj);
                model.BankNames = await BankNameSelectItems();
                model.RegionNames = await RegionSelectItems();
                model.DivisionNames = await DivisionSelectItems();
                model.TownshipNames = await TownshipSelectItems();
                model.StatusNames = BankStatusSelectItems();
                return View("EditBankBranch", model);
            }
        }

        [HttpPost]
        [ActionName("Update")]
        public async Task<IActionResult> UpdateBankBranch(int id, UpdateBankBranchViewModel model)
        {
            var query = _dBContext.BankBranchNames.AsNoTracking();
            var bankBranchObj = await query.FirstOrDefaultAsync(x => x.BankBranchNameId.Equals(id));

            ResponseModel response = new();

            if (bankBranchObj is null)
            {
                response.IsSuccess = false;
                response.Message = "Not found!";
            }
            else
            {
                var obj = ChangeBankBranchNameModel(id, model);
                _dBContext.BankBranchNames.Update(obj);
                int effectRow = await _dBContext.SaveChangesAsync();

                response.IsSuccess = effectRow > 0;
                response.Message = effectRow > 0 ? "Bank branch has been successfully updated" : "Error: Update bank failed!";
            }

            // Serialize your data using the specified options
            var data = System.Text.Json.JsonSerializer.Serialize(response, _jsonOption);
            return Content(data, MediaTypeNames.Application.Json);
        }

        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteBankBranch(int id)
        {
            var query = _dBContext.BankBranchNames.AsNoTracking();
            var bankBranchObj = await query.FirstOrDefaultAsync(x => x.BankBranchNameId.Equals(id));

            ResponseModel response = new();

            if (bankBranchObj is null)
            {
                response.IsSuccess = false;
                response.Message = "Not found!";
            }
            else
            {
                _dBContext.BankBranchNames.Remove(bankBranchObj);
                int effectRow = await _dBContext.SaveChangesAsync();

                response.IsSuccess = effectRow > 0;
                response.Message = effectRow > 0 ? "Bank branch has been successfully deleted" : "Error: Deleting bank failed!";
            }

            // Serialize your data using the specified options
            var data = System.Text.Json.JsonSerializer.Serialize(response, _jsonOption);
            return Content(data, MediaTypeNames.Application.Json);
        }

        #region Select Items
        private async Task<List<SelectListItem>> BankNameSelectItems()
        {
            var query = _dBContext.BankNames.AsNoTracking();
            return await query.Select(x => new SelectListItem() { Value = x.BankNameId.ToString(), Text = x.Name }).ToListAsync();
        }

        private async Task<List<SelectListItem>> RegionSelectItems()
        {
            var query = _dBContext.Regions.AsNoTracking();
            return await query.Select(x => new SelectListItem() { Value = x.RegionId.ToString(), Text = x.Name }).ToListAsync();
        }

        private async Task<List<SelectListItem>> DivisionSelectItems()
        {
            var query = _dBContext.Divisions.AsNoTracking();
            return await query.Select(x => new SelectListItem() { Value = x.DivisionId.ToString(), Text = x.Name }).ToListAsync();
        }

        private async Task<List<SelectListItem>> TownshipSelectItems()
        {
            var query = _dBContext.Townships.AsNoTracking();
            return await query.Select(x => new SelectListItem() { Value = x.TownshipId.ToString(), Text = x.Name }).ToListAsync();
        }

        [HttpPost]
        public async Task<IActionResult> DivisionSelectItemsByRegionId(int regionId)
        {
            var query = _dBContext.Divisions.AsNoTracking();
            List<SelectListItem> divisons = await query.Where(x => x.RegionId.Equals(regionId)).Select(x => new SelectListItem() { Value = x.DivisionId.ToString(), Text = x.Name }).ToListAsync();

            var data = System.Text.Json.JsonSerializer.Serialize(divisons, _jsonOption);
            return Content(data, MediaTypeNames.Application.Json);
        }

        [HttpPost]
        public async Task<IActionResult> TownshipSelectItemsByDivisionId(int divisionId)
        {
            var query = _dBContext.Townships.AsNoTracking();
            List<SelectListItem> townships = await query.Where(x => x.DivisionId.Equals(divisionId)).Select(x => new SelectListItem() { Value = x.DivisionId.ToString(), Text = x.Name }).ToListAsync();

            var data = System.Text.Json.JsonSerializer.Serialize(townships, _jsonOption);
            return Content(data, MediaTypeNames.Application.Json);
        }

        private List<SelectListItem> BankStatusSelectItems()
        {
            return
            [
                new()
                {
                    Value = ((int)EBankBranchStatus.Open).ToString(),
                    Text = Helper.GetEnumDescriptionByValue<EBankBranchStatus>((int)EBankBranchStatus.Open)
                },
                new()
                {
                    Value = ((int)EBankBranchStatus.TemporaryClosed).ToString(),
                    Text = Helper.GetEnumDescriptionByValue<EBankBranchStatus>((int)EBankBranchStatus.TemporaryClosed)
                }
            ];
        }
        #endregion

        #region Change
        private static BankBranchName ChangeBankBranchNameModel(CreateBankBranchViewModel model)
        {
            return new BankBranchName()
            {
                Name = model.Name,
                Code = model.Code,
                TelephoneNumber = model.TelephoneNumber,
                Address = model.Address,
                BankNameId = model.BankNameId,
                RegionId = model.RegionId,
                DivisionId = model.DivisionId,
                TownshipId = model.TownshipId,
                OpeningHour = model.OpeningHour,
                ClosedHour = model.ClosedHour,
                Status = model.Status
            };
        }

        private static BankBranchViewModel ChangeBankBranchViewModel(BankBranchName model)
        {
            return new BankBranchViewModel
            {
                BankBranchNameId = model.BankBranchNameId,
                Name = model.Name,
                Code = model.Code,
                TelephoneNumber = model.TelephoneNumber,
                Address = model.Address,
                BankNameId = model.BankNameId,
                RegionId = model.RegionId,
                RegionName = model.Region.Name,
                DivisionId = model.DivisionId,
                DivisionName = model.Division.Name,
                TownshipId = model.TownshipId,
                TownshipName = model.Township.Name,
                OpeningHour = model.OpeningHour,
                ClosedHour = model.ClosedHour,
                Status = model.Status,
                StatusDescription = Helper.GetEnumDescriptionByValue<EBankBranchStatus>(model.Status)
            };
        }

        private static BankBranchName ChangeBankBranchNameModel(int id, UpdateBankBranchViewModel model)
        {
            return new BankBranchName()
            {
                BankBranchNameId = id,
                Name = model.Name,
                Code = model.Code,
                TelephoneNumber = model.TelephoneNumber,
                Address = model.Address,
                BankNameId = model.BankNameId,
                RegionId = model.RegionId,
                DivisionId = model.DivisionId,
                TownshipId = model.TownshipId,
                OpeningHour = model.OpeningHour,
                ClosedHour = model.ClosedHour,
                Status = model.Status
            };
        }
        #endregion

    }
}
