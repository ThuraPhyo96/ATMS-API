using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ATMS.Web.Dto.Models
{
    public class BalanceHistory
    {
        public int BalanceHistoryId { get; set; }
        public Guid BalanceHistoryGuid { get; set; }

        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        public int BankAccountId { get; set; }
        public BankAccount BankAccount { get; set; }

        [DataType(DataType.Date)]
        public DateTime TransactionDate { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        public int HistoryType { get; set; }
    }

    [Flags]
    public enum EBalanceHistoryType
    {
        [Description("Withdraw")]
        Withdraw = 1,
        [Description("Deposite")]
        Deposite = 2
    }
}
