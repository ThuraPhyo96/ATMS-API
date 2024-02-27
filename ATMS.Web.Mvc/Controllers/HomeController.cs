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

                var response = await client.PostAsync($"{BaseUrl}/atmcard", new StringContent(bankCardJson, Encoding.UTF8, "application/json"));

                if (response.IsSuccessStatusCode)
                {
                    string responseJson = await response.Content.ReadAsStringAsync();
                    BankAccountDto result = JsonConvert.DeserializeObject<BankAccountDto>(responseJson);
                    TempData[StatusMessage.BankCardInfo] = responseJson;
                    return RedirectToAction("CardAction");
                }
                else
                {
                    TempData[StatusMessage.ActionStatusMessage] = JsonConvert.DeserializeObject<dynamic>(response.Content.ReadAsStringAsync().Result);
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
                using var bankCardClient = new HttpClient();
                bankCardClient.DefaultRequestHeaders.Accept.Clear();
                var getBankCardResponse = await bankCardClient.GetAsync($"{BaseUrl}/atmcard/{input.BankCardNumber}/false");

                if (getBankCardResponse.IsSuccessStatusCode)
                {
                    string bankCardResponseJson = await getBankCardResponse.Content.ReadAsStringAsync();
                    BankCardDto bankCard = JsonConvert.DeserializeObject<BankCardDto>(bankCardResponseJson);
                    input.PIN = bankCard.PIN;

                    string balanceJson = JsonConvert.SerializeObject(input);
                    using var client = new HttpClient();
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var response = await client.PutAsync($"{BaseUrl}/atmcard", new StringContent(balanceJson, Encoding.UTF8, "application/json"));

                    if (response.IsSuccessStatusCode)
                    {
                        string responseJson = await response.Content.ReadAsStringAsync();
                        BankAccountDto result = JsonConvert.DeserializeObject<BankAccountDto>(responseJson);
                        TempData[StatusMessage.BankCardInfo] = responseJson;
                        TempData[StatusMessage.ActionStatusMessage] = result.StatusMessage;
                    }
                    else
                    {
                        TempData[StatusMessage.ActionStatusMessage] = JsonConvert.DeserializeObject<dynamic>(response.Content.ReadAsStringAsync().Result);
                    }
                }
                else
                {
                    TempData[StatusMessage.ActionStatusMessage] = JsonConvert.DeserializeObject<dynamic>(getBankCardResponse.Content.ReadAsStringAsync().Result);
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

                var response = await client.GetAsync($"{BaseUrl}/atmcard/{cardNumber}/true");

                if (response.IsSuccessStatusCode)
                {
                    string responseJson = await response.Content.ReadAsStringAsync();
                    BalanceHistoryByCardNumberDto result = JsonConvert.DeserializeObject<BalanceHistoryByCardNumberDto>(responseJson);
                    return PartialView("_ShowHistoryModal", result);
                }
                else
                {
                    TempData[StatusMessage.ActionStatusMessage] = JsonConvert.DeserializeObject<dynamic>(response.Content.ReadAsStringAsync().Result);
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
