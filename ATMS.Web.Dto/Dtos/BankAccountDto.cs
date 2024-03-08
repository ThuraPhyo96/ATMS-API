using ATMS.Web.Dto.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ATMS.Web.Dto.Dtos
{
    public class BankAccountDto : ResponseDto
    {
        public string BankCardNumber { get; set; }

        [Display(Name = "Balance")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal AvailableBalance { get; set; }
        public int ActionType { get; set; }

        [Display(Name = "Balance")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        public BankAccountDto()
        {

        }

        public BankAccountDto(int statusCode, string statusMessage, string bankCardNumber, decimal balance)
        {
            StatusCode = statusCode;
            StatusMessage = statusMessage;
            BankCardNumber = bankCardNumber;
            AvailableBalance = balance;
        }
    }

    public class UpdateBalanceByCustomerDto
    {
        public string BankCardNumber { get; set; }

        [Required]
        [Display(Name = "PIN")]
        [DataType(DataType.Password)]
        [StringLength(6)]
        public string PIN { get; set; }
        public int ActionType { get; set; }
        public decimal Amount { get; set; }
    }

    public class BankAccountSingleDto
    {
        public int BankAccountId { get; set; }

        public int CustomerId { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Balance { get; set; }
    }
}
