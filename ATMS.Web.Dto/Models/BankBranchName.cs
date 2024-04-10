using System;
using System.ComponentModel.DataAnnotations;

namespace ATMS.Web.Dto.Models
{
    public class BankBranchName
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
        public Region Region { get; set; }

        public int DivisionId { get; set; }
        public Division Division { get; set; }

        public int TownshipId { get; set; }
        public Township Township { get; set; }

        public TimeSpan OpeningHour { get; set; }

        public TimeSpan ClosedHour { get; set; }

        public int Status { get; set; }

        public BankBranchName()
        {
                
        }

        public BankBranchName(int bankNameId, string name, string code, string telephoneNumber, string address, int regionId,
               int divisionId, int townshipId, TimeSpan openingHour, TimeSpan closedHour, int status)
        {
            BankNameId = bankNameId;
            Name = name;
            Code = code;
            TelephoneNumber = telephoneNumber;
            Address = address;
            RegionId = regionId;
            DivisionId = divisionId;
            TownshipId = townshipId;
            OpeningHour = openingHour;
            ClosedHour = closedHour;
            Status = status;
        }
    }

    public enum EBankBranchStatus
    {
        Open = 1,
        TemporaryClosed = 2
    }
}
