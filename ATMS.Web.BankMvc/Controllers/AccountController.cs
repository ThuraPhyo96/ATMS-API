using ATM.Web.ViewModels;
using ATMS.Web.Dto.Dtos;
using ATMS.Web.Shared;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace ATMS.Web.BankMvc.Controllers
{
    public class AccountController : Controller
    {
        private readonly DapperService _dapperService;

        public AccountController(DapperService dapperService)
        {
            _dapperService = dapperService;
        }

        [ActionName("Index")]
        public IActionResult Login(string username, string password)
        {
            LoginViewModel model = new();
            if (!string.IsNullOrEmpty(username))
                model.Username = username;

            if (!string.IsNullOrEmpty(password))
                model.Password = password;

            return View("LoginIndex", model);
        }

        [HttpPost]
        [ActionName("PostLogin")]
        public IActionResult Login(LoginViewModel model)
        {
            (string query, Dictionary<string, object> parameters) = GetQueryAndParameters(model.Username, model.Password);
            var dtos = _dapperService.Query<UserDto>(query, parameters);
            var record = dtos.FirstOrDefault();

            if (record is null)
                return RedirectToAction("Index", "Account", new { username = model.Username, password = model.Password });
            else
                return RedirectToAction("Index", "Home");
        }

        private static (string, Dictionary<string, object>) GetQueryAndParameters(string username, string password)
        {
            string query = @"SELECT * FROM Users WHERE Username = @Username AND Password = @Password";

            Dictionary<string, object> parameters = new()
            {
                { "@Username", username },
                { "@Password", password }
            };

            return (query, parameters);
        }
    }
}
