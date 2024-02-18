using ATMS.Web.API.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace ATMS.Web.API.AppServices.Dtos
{
    public class BalanceHistoryDto
    {
        public int BalanceHistoryId { get; set; }
        public Guid BalanceHistoryGuid { get; set; }

        public int CustomerId { get; set; }

        public int BankAccountId { get; set; }

        public DateTime TransactionDate { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        public int HistoryType { get; set; }

        public bool IsActive { get; set; }

        [StringLength(MaxLength.L_500)]
        public string Remark { get; set; }
    }

    public class CreateBalanceHistoryDto
    {
        public int CustomerId { get; set; }
        public int BankAccountId { get; set; }
        public DateTime TransactionDate { get; set; } = DateTime.Today;

        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }
        public int HistoryType { get; set; }
        public bool IsActive { get; set; } = true;

        public CreateBalanceHistoryDto()
        {

        }

        public CreateBalanceHistoryDto(int customerId, int bankAccountId, decimal amount, int historyType)
        {
            CustomerId = customerId;
            BankAccountId = bankAccountId;
            Amount = amount;
            HistoryType = historyType;
        }
    }

    public class BalanceHistoryByCardNumberDto
    {
        public int StatusCode { get; set; }
        public string StatusMessage { get; set; }
        public IReadOnlyList<BalanceHistoryDto> Histories { get; set; } = new List<BalanceHistoryDto>();

        public BalanceHistoryByCardNumberDto()
        {

        }

        public BalanceHistoryByCardNumberDto(int statusCode, string statusMessage, IReadOnlyList<BalanceHistoryDto> histories)
        {
            StatusCode = statusCode;
            StatusMessage = statusMessage;
            Histories = histories;
        }
    }
}
