using ATMS.Web.Dto.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ATMS.Web.Dto.Dtos
{
    public class BankBranchNameDto
    {
        public int BankBranchNameId { get; set; }

        public int BankNameId { get; set; }
        public BankName BankName { get; set; }

        [StringLength(MaxLength.L_100)]
        public string Name { get; set; }

        [StringLength(MaxLength.L_12)]
        public string Code { get; set; }

        [StringLength(MaxLength.L_500)]
        public string TelephoneNumber { get; set; }

        [StringLength(MaxLength.L_500)]
        public string Address { get; set; }

        public int RegionId { get; set; }
        public RegionDto Region { get; set; }

        public int DivisionId { get; set; }
        public DivisionDto Division { get; set; }

        public int TownshipId { get; set; }
        public TownshipDto Township { get; set; }

        public TimeSpan OpeningHour { get; set; }

        public TimeSpan ClosedHour { get; set; }

        public int Status { get; set; }
    }
}
