using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ATMS.Web.Dto.Models
{
    public class ATMLocation
    {
        public int ATMLocationId { get; set; }

        public int BankNameId { get; set; }
        public BankName BankName { get; set; }

        public int? BankBranchNameId { get; set; }
        public BankBranchName BankBranchName { get; set; }

        public int RegionId { get; set; }
        public Region Region { get; set; }

        public int DivisionId { get; set; }
        public Division Division { get; set; }

        public int TownshipId { get; set; }
        public Township Township { get; set; }

        [StringLength(MaxLength.L_1000)]
        public string Address { get; set; }

        public int Status { get; set; }

        public ATMLocation()
        {

        }

        public ATMLocation(int bankNameId, int? bankBranchNameId, int regionId, int divisionId, int townshipId, string address, int status)
        {
            BankNameId = bankNameId;
            BankBranchNameId = bankBranchNameId;
            RegionId = regionId;
            DivisionId = divisionId;
            TownshipId = townshipId;
            Address = address;
            Status = status;
        }
    }

    public enum EATMStatus
    {
        Available = 1,
        TemporaryCloesd = 2
    }
}
