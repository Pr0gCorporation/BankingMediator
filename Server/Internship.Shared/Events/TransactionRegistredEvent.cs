namespace Internship.Shared.Events
{
    public class TransactionRegistredEvent
    {
        public string DebtorIBAN { get; set; }
        public string CreditorIBAN { get; set; }
        public decimal Amount { get; set; }
        public string Reference { get; set; }
    }
}
