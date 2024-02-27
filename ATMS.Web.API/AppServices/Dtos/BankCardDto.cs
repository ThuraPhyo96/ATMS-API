using ATMS.Web.API.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;

namespace ATMS.Web.API.AppServices.Dtos
{
    public class BankCardDto : ResponseDto
    {
        public int BankCardId { get; set; }
        public Guid BankCardGuid { get; set; }

        public int CustomerId { get; set; }

        public int BankAccountId { get; set; }

        [StringLength(MaxLength.L_20)]
        public string BankCardNumber { get; set; }

        public string PIN { get; set; }

        public DateTime ValidDate { get; set; }

        public BankCardDto()
        {
                
        }

        public BankCardDto(int statusCode, string message)
        {
            StatusCode = statusCode;
            StatusMessage = message;
        }
    }

    public class CheckBankCardDto
    {
        [Required]
        [Display(Name = "Card Number")]
        public string BankCardNumber { get; set; }

        [Required]
        [Display(Name = "PIN")]
        [DataType(DataType.Password)]
        [StringLength(6)]
        public string PIN { get; set; }
    }
}
