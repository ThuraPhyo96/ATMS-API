using System.ComponentModel.DataAnnotations;

namespace ATMS.Web.API.AppServices.Dtos
{
    public class BankCardDto
    {
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
