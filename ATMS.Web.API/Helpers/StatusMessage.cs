namespace ATMS.Web.API.Helpers
{
    public static class StatusMessage
    {
        public const string ValidCard = "Valid card";
        public const string InvalidCard = "Error: Invalid card";
        public const string InvalidAccount = "Error: Invalid account";

        public const string WithdrawSuccess = "Withdraw successfully.";
        public const string DepositSuccess = "Deposit successfully.";

        public const string NotEnoughAmount = "Error: Your withdraw money is not enouth!";
        public const string Error = "Error: One or more validation errors occurred.";

        public const string ActionMessagePartial = "_ShowActionMessagePartial";
        public const string ActionStatusMessage = "StatusMessage";
        public const string BankCardInfo = "BankCardInfo";
    }
}
