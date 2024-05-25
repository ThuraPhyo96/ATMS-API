using ATM.Web.ViewModels;
using ATMS.Web.Dto.Dtos;
using ATMS.Web.Shared;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Mime;
using System.Text.Json;

namespace ATMS.Web.BankMvc.Controllers
{
    public class UserController : Controller
    {
        private readonly DapperService _dapperService;
        private readonly JsonSerializerOptions _jsonOption;

        public UserController(DapperService dapperService)
        {
            _dapperService = dapperService;
            // Configure serialization options
            _jsonOption = new()
            {
                PropertyNamingPolicy = null // Disable any naming policy
            };
        }

        [ActionName("Index")]
        public IActionResult UserIndex()
        {
            string query = @"SELECT * FROM Users;";

            var dtos = _dapperService.Query<UserDto>(query);
            var model = dtos.Select(x => ChangeToViewModel(x)).ToList();
            return View("UserIndex", model);
        }

        [HttpGet]
        [ActionName("Create")]
        public IActionResult CreateUser()
        {
            CreateUserViewModel model = new();
            return View("CreateUser", model);
        }

        [HttpPost]
        [ActionName("Save")]
        public IActionResult SaveUser(CreateUserViewModel model)
        {
            (string query, Dictionary<string, object> parameters) = GetCreateQueryAndParameters(model);
            var effectRow = _dapperService.Execute(query, parameters);

            ResponseModel response = new()
            {
                IsSuccess = effectRow > 0,
                Message = effectRow > 0 ? "User has been successfully created" : "Error: Creation user failed!"
            };

            // Serialize your data using the specified options
            string data = JsonSerializer.Serialize(response, _jsonOption);

            return Content(data, MediaTypeNames.Application.Json);
        }

        [HttpGet]
        [ActionName("Edit")]
        public IActionResult EditUser(string id)
        {
            string query = @"SELECT * FROM Users WHERE UserId = @UserId;";
            Dictionary<string, object>? parameters = new()
            {
                { "@UserId", id }
            };
            var dtos = _dapperService.Query<UserDto>(query, parameters);
            var record = dtos.FirstOrDefault();

            if (record is null)
                return BadRequest();
            else
            {
                var model = ChangeToEditViewModel(record);
                return View("EditUser", model);
            }
        }

        [HttpPost]
        [ActionName("Update")]
        public IActionResult UpdateUser(string id, EditUserViewModel model)
        {
            string query = @"SELECT * FROM Users WHERE UserId = @UserId;";
            Dictionary<string, object>? parameters = new()
            {
                { "@UserId", id }
            };
            var dtos = _dapperService.Query<UserDto>(query, parameters);
            var record = dtos.FirstOrDefault();
            ResponseModel response = new();

            if (record is null)
            {
                response.IsSuccess = false;
                response.Message = "Not found!";
            }
            else
            {
                (string updateQuery, Dictionary<string, object> updateParameters) = GetUpdateQueryAndParameters(id, model);
                var effectRow = _dapperService.Execute(updateQuery, updateParameters);

                response.IsSuccess = effectRow > 0;
                response.Message = effectRow > 0 ? "User has been successfully updated" : "Error: Update user failed!";
            }

            // Serialize your data using the specified options
            var data = JsonSerializer.Serialize(response, _jsonOption);
            return Content(data, MediaTypeNames.Application.Json);
        }

        private static UserViewModel ChangeToViewModel(UserDto input)
        {
            return new UserViewModel()
            {
                UserId = input.UserId,
                Username = input!.Username,
                Password = new string('*', input.Password.Length)
            };
        }

        private static EditUserViewModel ChangeToEditViewModel(UserDto input)
        {
            return new EditUserViewModel()
            {
                UserId = input.UserId,
                Username = input.Username,
                Password = input.Password
            };
        }

        private static (string, Dictionary<string, object>) GetCreateQueryAndParameters(CreateUserViewModel model)
        {
            string query = @"INSERT INTO [dbo].[Users]
                                               ([UserId]
                                               ,[Username]
                                               ,[Password]                                              
                                               )
                                         VALUES
                                               (
                                               @UserId
                                               ,@Username
                                               ,@Password
                                               )";

            Dictionary<string, object> parameters = new()
            {
                { "@UserId",Guid.NewGuid().ToString() },
                { "@Username", model.Username },
                { "@Password", model.Password }
            };

            return (query, parameters);
        }

        private static (string, Dictionary<string, object>) GetUpdateQueryAndParameters(string id, EditUserViewModel model)
        {
            string query = @"UPDATE [dbo].[Users]
                            SET [Username] = @Username
                                ,[Password] = @Password
                            WHERE UserId = @UserId;";

            Dictionary<string, object> parameters = new()
            {
                { "@UserId", id },
                { "@Username", model.Username },
                { "@Password", model.Password }
            };

            return (query, parameters);
        }
    }
}
