using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace ATMS.Web.Dto.Models
{
    public class SpendTransaction
    {
        public int SpendTransactionId { get; set; }

        public int TransactionType { get; set; }

        public DateTime TransactionDate { get; set; }

        public string Category { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        public string Note { get; set; }
    }

    public enum ESpendTransactionTypes
    {
        [Description("Expense")]
        Expense = 1,
        [Description("Income")]
        Income = 2
    }
}
