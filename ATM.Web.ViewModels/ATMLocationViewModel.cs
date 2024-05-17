using ATMS.Web.Dto.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ATM.Web.ViewModels
{
    public class ATMLocationViewModel
    {
        public int ATMLocationId { get; set; }
        public string BankName { get; set; }
        public string BankBranchName { get; set; }
        public string RegionName { get; set; }
        public string DivisionName { get; set; }
        public string TownshipName { get; set; }
        public string Address { get; set; }
        public string Status { get; set; }

        public List<SelectListItem> BankNames { get; set; }
        public List<SelectListItem> BankBranchNames { get; set; }
        public List<SelectListItem> RegionNames { get; set; }
        public List<SelectListItem> DivisionNames { get; set; }
        public List<SelectListItem> TownshipNames { get; set; }
        public List<SelectListItem> StatusNames { get; set; }

    }

    public class CreateATMLocationViewModel
    {
        [Required]
        [Display(Name = "Bank Name")]
        public int BankNameId { get; set; }

        [Display(Name = "Bank Branch Name")]
        public int? BankBranchNameId { get; set; }

        [Required]
        [Display(Name = "Region")]
        public int RegionId { get; set; }

        [Required]
        [Display(Name = "Division")]
        public int DivisionId { get; set; }

        [Required]
        [Display(Name = "Township")]
        public int TownshipId { get; set; }

        [Required]
        [Display(Name = "Address")]
        [StringLength(MaxLength.L_1000)]
        public string Address { get; set; }

        [Required]
        [Display(Name = "Status")]
        public int Status { get; set; }

        public List<SelectListItem> BankNames { get; set; }
        public List<SelectListItem> RegionNames { get; set; }
        public List<SelectListItem> StatusNames { get; set; }
    }

    public class UpdateATMLocationViewModel
    {
        public int BankNameId { get; set; }
        public int? BankBranchNameId { get; set; }
        public int RegionId { get; set; }
        public int DivisionId { get; set; }
        public int TownshipId { get; set; }
        public string Address { get; set; }
        public int Status { get; set; }
    }
}