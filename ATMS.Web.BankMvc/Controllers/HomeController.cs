using ATM.Web.ViewModels;
using ATMS.Web.BankMvc.Data;
using ATMS.Web.BankMvc.Models;
using ATMS.Web.Dto.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace ATMS.Web.BankMvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDBContext _dBContext;

        public HomeController(ILogger<HomeController> logger, ApplicationDBContext dBContext)
        {
            _logger = logger;
            _dBContext = dBContext;
        }

        public async Task<IActionResult> IndexAsync()
        {
            var objs = _dBContext.BankNames
                  .Include(x => x.BankBranchNames)
                  .ThenInclude(x => x.Township)
                  .AsNoTracking()
                  .OrderBy(x => x.Name);

            var bankByTownship = ChangeBankViewModel(await objs.ToListAsync());
            var bankBranchesByBank = ChangeBankSummaryViewModel(await objs.ToListAsync());
            var model = new DashboardViewModel()
            {
                BankViewModel = bankBranchesByBank,
                TownshipViewModel = bankByTownship
            };
            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        #region Change
        private DashboardSummaryViewModel ChangeBankSummaryViewModel(List<BankName> bankNames)
        {
            List<string> names = [];
            List<int> branches = [];

            bankNames.ForEach(
                bank =>
                {
                    names.Add(bank.Name);
                    branches.Add(bank.BankBranchNames.Count);
                }
                );

            return new DashboardSummaryViewModel()
            {
                Labels = names,
                Series = branches
            };
        }

        private DashboardSummaryViewModel ChangeBankViewModel(List<BankName> bankNames)
        {
            List<string> labels = [];
            List<int> series = [];

            bankNames.ForEach(
                bankName =>
                {
                    var townships = bankName.BankBranchNames.GroupBy(x => new { x.TownshipId, x.Township.Name });
                    foreach (var item in townships)
                    {
                        labels.Add(item.Select(x => x.Township.Name).FirstOrDefault()!);
                        series.Add(item.Count());
                    }
                }
                );
            return new DashboardSummaryViewModel() { Labels = labels, Series = series };
        }
        #endregion
    }
}
