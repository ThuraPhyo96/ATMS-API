using ATM.Web.ViewModels;
using ATMS.Web.Dto.Dtos;
using ATMS.Web.Dto.Models;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATMS.ConsoleApp.RefitExamples
{
    public interface IRefitExample
    {
        [Post("/atmcard/")]
        Task<BankAccountDto> CheckBankCard([Body] BankCard input);

        [Put("/atmcard/")]
        Task<BankAccountDto> UpdateBalanceByCustomer([Body] UpdateBalanceByCustomerDto input);

        [Get("/atmcard/{cardNumber}/{viewHistory}")]
        Task<BalanceHistoryByCardNumberDto> BalanceHistoryByCardNumber(string cardNumber, bool viewHistory);

        [Get("/atmcard/{bankName}")]
        Task<List<ATMLocationViewModel>> ATMLocationByBankName(string bankName);
    }
}
