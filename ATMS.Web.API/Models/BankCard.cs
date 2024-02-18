using System.ComponentModel.DataAnnotations;
using System;

namespace ATMS.Web.API.Models
{
    public class BankCard
    {
        public int BankCardId { get; set; }
        public Guid BankCardGuid { get; set; }

        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        public int BankAccountId { get; set; }
        public BankAccount BankAccount { get; set; }

        [StringLength(MaxLength.L_20)]
        public string BankCardNumber { get; set; }

        public string PIN { get; set; }

        public DateTime ValidDate { get; set; }
    }
}
