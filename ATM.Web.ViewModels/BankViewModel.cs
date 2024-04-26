using ATMS.Web.Dto.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ATM.Web.ViewModels
{
    public class BankViewModel
    {
        public int BankNameId { get; set; }

        [Required]
        [StringLength(MaxLength.L_100)]
        public string Name { get; set; }

        [Required]
        [StringLength(MaxLength.L_12)]
        public string Code { get; set; }

        [Required]
        [StringLength(MaxLength.L_20)]
        public string TelephoneNumber { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(MaxLength.L_100)]
        public string Email { get; set; }

        [Required]
        [StringLength(MaxLength.L_500)]
        public string Address { get; set; }

        public bool IsActive { get; set; }
    }

    public class CreateBankViewModel
    {
        [DisplayName("Name")]
        [Required]
        [StringLength(MaxLength.L_100)]
        public string Name { get; set; }

        [DisplayName("Code")]
        [Required]
        [StringLength(MaxLength.L_12)]
        public string Code { get; set; }

        [DisplayName("TelephoneNumber")]
        [Required]
        [StringLength(MaxLength.L_20)]
        public string TelephoneNumber { get; set; }

        [DisplayName("Email")]
        [Required]
        [EmailAddress]
        [StringLength(MaxLength.L_100)]
        public string Email { get; set; }

        [DisplayName("Address")]
        [Required]
        [StringLength(MaxLength.L_500)]
        public string Address { get; set; }

        [DisplayName("Enabled?")]
        public bool IsActive { get; set; }
    }

    public class ResponseModel
    {
        public bool IsSuccess { get; set; }

        public string Message { get; set; }
    }
}
