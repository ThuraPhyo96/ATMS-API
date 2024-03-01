using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ATMS.Web.Dto.Models
{
    public class BankAccount
    {
        public int BankAccountId { get; set; }
        public Guid BankAccountGuid { get; set; }

        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        [StringLength(MaxLength.L_20)]
        public string AccountNumber { get; set; }

        public int AccountType { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Balance { get; set; }

        public bool IsActive { get; set; }

        public BankCard BankCard { get; set; }
    }

    [Flags]
    public enum EBankAccountType
    {
        [Description("e-Saving Account")]
        Saving = 1,
        [Description("Special Account")]
        Special = 2
    }
}
