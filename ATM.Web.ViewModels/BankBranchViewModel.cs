using ATMS.Web.Dto.Dtos;
using ATMS.Web.Dto.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ATM.Web.ViewModels
{
    public class BankBranchViewModel
    {
        public int BankBranchNameId { get; set; }

        [Required]
        [DisplayName("Bank Name")]
        public int BankNameId { get; set; }
        public string BankName { get; set; }

        [Required]
        [DisplayName("Name")]
        [StringLength(MaxLength.L_100)]
        public string Name { get; set; }

        [Required]
        [DisplayName("Code")]
        [StringLength(MaxLength.L_12)]
        public string Code { get; set; }

        [Required]
        [DisplayName("Telephone Number")]
        [StringLength(MaxLength.L_500)]
        public string TelephoneNumber { get; set; }

        [Required]
        [DisplayName("Address")]
        [StringLength(MaxLength.L_500)]
        public string Address { get; set; }

        [Required]
        [DisplayName("Region Name")]
        public int RegionId { get; set; }
        public string RegionName { get; set; }

        [Required]
        [DisplayName("Division Name")]
        public int DivisionId { get; set; }
        public string DivisionName { get; set; }

        [Required]
        [DisplayName("Township Name")]
        public int TownshipId { get; set; }
        public string TownshipName { get; set; }

        [Required]
        [DisplayName("Opening Hour")]
        public TimeSpan OpeningHour { get; set; }

        [Required]
        [DisplayName("Closed Hour")]
        public TimeSpan ClosedHour { get; set; }

        [DisplayName("Status")]
        public int Status { get; set; }
        public string StatusDescription { get; set; }

        public List<SelectListItem> BankNames { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> RegionNames { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> DivisionNames { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> TownshipNames { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> StatusNames { get; set; } = new List<SelectListItem>();
    }

    public class CreateBankBranchViewModel
    {
        [Required]
        [DisplayName("Bank Name")]
        public int BankNameId { get; set; }
        public string BankName { get; set; }

        [Required]
        [DisplayName("Name")]
        [StringLength(MaxLength.L_100)]
        public string Name { get; set; }

        [Required]
        [DisplayName("Code")]
        [StringLength(MaxLength.L_12)]
        public string Code { get; set; }

        [Required]
        [DisplayName("Telephone Number")]
        [StringLength(MaxLength.L_500)]
        public string TelephoneNumber { get; set; }

        [Required]
        [DisplayName("Address")]
        [StringLength(MaxLength.L_500)]
        public string Address { get; set; }

        [Required]
        [DisplayName("Region Name")]
        public int RegionId { get; set; }
        public string RegionName { get; set; }

        [Required]
        [DisplayName("Division Name")]
        public int DivisionId { get; set; }
        public string DivisionName { get; set; }

        [Required]
        [DisplayName("Township Name")]
        public int TownshipId { get; set; }
        public string Township { get; set; }

        [Required]
        [DisplayName("Opening Hour")]
        public TimeSpan OpeningHour { get; set; }

        [Required]
        [DisplayName("Closed Hour")]
        public TimeSpan ClosedHour { get; set; }

        [Required]
        [DisplayName("Status")]
        public int Status { get; set; }

        public List<SelectListItem> BankNames { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> RegionNames { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> StatusNames { get; set; } = new List<SelectListItem>();
    }

    public class UpdateBankBranchViewModel
    {
        public int BankNameId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string TelephoneNumber { get; set; }
        public string Address { get; set; }
        public int RegionId { get; set; }
        public int DivisionId { get; set; }
        public int TownshipId { get; set; }
        public TimeSpan OpeningHour { get; set; }
        public TimeSpan ClosedHour { get; set; }
        public int Status { get; set; }
    }

    public class BankBranchListViewModel
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int PageCount { get; set; }
        public bool IsEndOfPage => PageNumber == PageCount;
        public List<BankBranchViewModel> Data { get; set; } = new List<BankBranchViewModel>();
    }
}
