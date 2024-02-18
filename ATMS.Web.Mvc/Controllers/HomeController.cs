using ATMS.Web.API.AppServices.Dtos;
using ATMS.Web.API.Helpers;
using ATMS.Web.Mvc.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient.Server;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ATMS.Web.Mvc.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(IHttpClientFactory httpClientFactory) : base(httpClientFactory) { }

        public IActionResult Index()
        {
            return View(new CheckBankCardDto());
        }

        [HttpPost]
        public async Task<IActionResult> SubmitCard(CheckBankCardDto bankCardDto)
        {
            try
            {
                string bankCardJson = JsonConvert.SerializeObject(bankCardDto);
                using var client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.PostAsync($"{BaseUrl}/atmcard/cardlogin", new StringContent(bankCardJson, Encoding.UTF8, "application/json"));

                string responseJson = await response.Content.ReadAsStringAsync();
                BankAccountDto result = JsonConvert.DeserializeObject<BankAccountDto>(responseJson);

                if (result.StatusCode == StatusCodes.Status200OK)
                {
                    TempData[StatusMessage.BankCardInfo] = responseJson;
                    return RedirectToAction("CardAction");
                }
                else
                {
                    TempData[StatusMessage.ActionStatusMessage] = result.StatusMessage;
                }
            }
            catch (ArgumentNullException)
            {
                TempData[StatusMessage.ActionStatusMessage] = StatusMessage.InvalidCard;
            }

            return View(nameof(Index));
        }

        public IActionResult CardAction()
        {
            BankAccountDto bankAccount = new BankAccountDto();

            if (TempData[StatusMessage.BankCardInfo] == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                try
                {
                    bankAccount = JsonConvert.DeserializeObject<BankAccountDto>(TempData[StatusMessage.BankCardInfo].ToString());
                }
                catch
                {
                    return RedirectToAction("Index");
                }
            }

            return View(bankAccount);
        }

        public ActionResult UpdateBalanceModal(BankAccountDto input)
        {
            return PartialView("_UpdateBalanceModal", input);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdateBalance(UpdateBalanceByCustomerDto input)
        {
            try
            {
                string balanceJson = JsonConvert.SerializeObject(input);
                using var client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.PostAsync($"{BaseUrl}/atmcard/updatebalance", new StringContent(balanceJson, Encoding.UTF8, "application/json"));

                string responseJson = await response.Content.ReadAsStringAsync();
                BankAccountDto result = JsonConvert.DeserializeObject<BankAccountDto>(responseJson);

                if (result.StatusCode == StatusCodes.Status200OK)
                {
                    TempData[StatusMessage.BankCardInfo] = responseJson;
                    TempData[StatusMessage.ActionStatusMessage] = result.StatusMessage;
                }
                else
                {
                    TempData[StatusMessage.ActionStatusMessage] = result.StatusMessage;
                }
            }
            catch
            {
                return View("CardAction");
            }
            return RedirectToAction("CardAction");
        }

        public async Task<ActionResult> ShowHistoryModal(string cardNumber)
        {
            try
            {
                string balanceJson = JsonConvert.SerializeObject(cardNumber);
                using var client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.PostAsync($"{BaseUrl}/atmcard/gethistorybycardnumber", new StringContent(balanceJson, Encoding.UTF8, "application/json"));

                string responseJson = await response.Content.ReadAsStringAsync();
                BalanceHistoryByCardNumberDto result = JsonConvert.DeserializeObject<BalanceHistoryByCardNumberDto>(responseJson);

                if (result.StatusCode == StatusCodes.Status200OK)
                {
                    return PartialView("_ShowHistoryModal", result);
                }
                else
                {
                    TempData[StatusMessage.ActionStatusMessage] = result.StatusMessage;
                }
            }
            catch
            {
                return View("CardAction");
            }
            return RedirectToAction("CardAction");
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
    }
}
