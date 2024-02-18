using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;

namespace ATMS.Web.API.Models
{
    public class Customer
    {
        public int CustomerId { get; set; }
        public Guid CustomerGuid { get; set; }

        [StringLength(MaxLength.L_100)]
        public string FullName { get; set; }

        public DateTime DateOfBirth { get; set; }

        [StringLength(MaxLength.L_50)]
        public string NRIC { get; set; }

        [StringLength(MaxLength.L_100)]
        public string FatherName { get; set; }

        [StringLength(MaxLength.L_12)]
        public string MobileNumber { get; set; }

        public BankAccount BankAccount { get; set; }
        public List<BalanceHistory> BalanceHistories { get; set; } = new List<BalanceHistory>();
    }
}
