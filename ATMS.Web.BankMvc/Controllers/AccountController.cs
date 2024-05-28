using ATM.Web.ViewModels;
using ATMS.Web.Dto.Dtos;
using ATMS.Web.Dto.Models;
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
        public IActionResult Login()
        {
            LoginViewModel model = new();
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
                return RedirectToAction("Index", "Account");

            DateTime sessionInteval = DateTime.Now.AddSeconds(45);
            var cookieOptions = new CookieOptions
            {
                Expires = sessionInteval,
            };

            (string createQuery, Dictionary<string, object> createParameters) = GetCreateQueryAndParameters(record.UserId, sessionInteval);
            var effectRow = _dapperService.Execute(createQuery, createParameters);
            if (effectRow > 0)
            {
                (string getQuery, Dictionary<string, object> getParameters) = GetUserSessionQueryAndParameters(record.UserId);
                var userSessions = _dapperService.Query<UserSessionDto>(getQuery, getParameters);
                var userSession = userSessions.FirstOrDefault();

                Response.Cookies.Append("UserId", userSession!.UserId.ToString(), cookieOptions);
                Response.Cookies.Append("UserSessionId", userSession!.UserSessionId.ToString(), cookieOptions);
            }

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

        private static (string, Dictionary<string, object>) GetCreateQueryAndParameters(Guid userId, DateTime sessionInterval)
        {
            string query = @"INSERT INTO [dbo].[UserSessions]
                                               ([UserSessionId]
                                                ,[UserId]
                                                ,[SessionInterval]
                                               )
                                         VALUES
                                               (@UserSessionId
                                               ,@UserId
                                               ,@SessionInterval
                                               )";

            Dictionary<string, object> parameters = new()
            {
                { "@UserSessionId", Guid.NewGuid() },
                { "@UserId", userId },
                { "@SessionInterval", sessionInterval}
            };

            return (query, parameters);
        }

        private static (string, Dictionary<string, object>) GetUserSessionQueryAndParameters(Guid userId)
        {
            string query = @"SELECT * FROM UserSessions WHERE UserId = @UserId";

            Dictionary<string, object> parameters = new()
            {
                { "@UserId", userId },
            };

            return (query, parameters);
        }

    }
}
