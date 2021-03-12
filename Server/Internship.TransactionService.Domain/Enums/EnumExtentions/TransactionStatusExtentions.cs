namespace Internship.TransactionService.Domain.Enums.EnumExtentions
{
    public static class TransactionStatusExtentions
    {
        public static string ToFriendlyString(this TransactionStatus me)
        {
            return me switch
            {
                TransactionStatus.Created => "created",
                TransactionStatus.Persisted => "persisted",
                TransactionStatus.Processing => "processing",
                TransactionStatus.Completed => "completed",
                TransactionStatus.Canceled => "canceled",
                _ => null
            };
        }
    }
}