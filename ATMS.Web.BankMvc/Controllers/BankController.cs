using ATM.Web.ViewModels;
using ATMS.Web.BankMvc.Data;
using ATMS.Web.Dto.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ATMS.Web.BankMvc.Controllers
{
    public class BankController : Controller
    {
        private readonly ApplicationDBContext _dbContext;

        public BankController()
        {
            _dbContext = new ApplicationDBContext();
        }

        [ActionName("Index")]
        public async Task<IActionResult> BankIndex()
        {
            var query = _dbContext.BankNames.AsNoTracking();
            var bankObjs = await query.OrderBy(x => x.Name).ToListAsync();

            List<BankViewModel> models = bankObjs.Select(x => ChangeBankViewModel(x)).ToList();
            return View("BankIndex", models);
        }

        [ActionName("Create")]
        public IActionResult CreateBank()
        {
            CreateBankViewModel model = new();
            return View("CreateBank", model);
        }

        [ActionName("Save")]
        [HttpPost]
        public async Task<IActionResult> SaveBank(CreateBankViewModel model)
        {
            BankName bankName = ChangeBankNameModel(model);
            _dbContext.BankNames.Add(bankName);
            int effectRows = await _dbContext.SaveChangesAsync();

            TempData["Message"] = effectRows > 0 ? "Bank has been successfully created" : "Error: Creation bank failed!";
            return RedirectToAction("Index");
        }

        [HttpGet]
        [ActionName("Edit")]
        public async Task<IActionResult> EditBank(int id)
        {
            var query = _dbContext.BankNames.AsNoTracking();
            var bankObj = await query.FirstOrDefaultAsync(x => x.BankNameId.Equals(id));

            var model = new BankViewModel();
            if (bankObj is not null)
                model = ChangeBankViewModel(bankObj);

            return View("EditBank", model);
        }

        [HttpPost]
        [ActionName("Update")]
        public async Task<IActionResult> UpdateBank(int id, UpdateBankViewModel model)
        {
            var query = _dbContext.BankNames.AsNoTracking();
            var bankObj = await query.FirstOrDefaultAsync(x => x.BankNameId.Equals(id));

            if (bankObj is not null)
            {
                BankName bankName = ChangeBankNameModel(id, model);
                _dbContext.BankNames.Update(bankName);
                int effectRows = await _dbContext.SaveChangesAsync();
                TempData["Message"] = effectRows > 0 ? "Bank has been successfully updated" : "Error: Updated bank failed!";
            }
            return RedirectToAction("Index");
        }

        [ActionName("Delete")]
        public async Task<IActionResult> DeleteBank(int id)
        {
            var query = _dbContext.BankNames.AsNoTracking();
            var bankObj = await query.FirstOrDefaultAsync(x => x.BankNameId.Equals(id));

            if (bankObj is not null)
            {
                _dbContext.BankNames.Remove(bankObj);
                int effectRows = await _dbContext.SaveChangesAsync();
                TempData["Message"] = effectRows > 0 ? "Bank has been successfully deleted" : "Error: Deleted bank failed!";
            }
            return RedirectToAction("Index");
        }

        private static BankName ChangeBankNameModel(CreateBankViewModel model)
        {
            return new BankName()
            {
                Name = model.Name,
                Code = model.Code,
                TelephoneNumber = model.TelephoneNumber,
                Email = model.Email,
                Address = model.Address,
                IsActive = model.IsActive
            };
        }

        private static BankViewModel ChangeBankViewModel(BankName model)
        {
            return new BankViewModel()
            {
                BankNameId = model.BankNameId,
                Name = model.Name,
                Code = model.Code,
                TelephoneNumber = model.TelephoneNumber,
                Email = model.Email,
                Address = model.Address,
                IsActive = model.IsActive
            };
        }

        private static BankName ChangeBankNameModel(int id, UpdateBankViewModel model)
        {
            return new BankName()
            {
                BankNameId = id,
                Name = model.Name,
                Code = model.Code,
                TelephoneNumber = model.TelephoneNumber,
                Email = model.Email,
                Address = model.Address,
                IsActive = model.IsActive
            };
        }
    }
}
