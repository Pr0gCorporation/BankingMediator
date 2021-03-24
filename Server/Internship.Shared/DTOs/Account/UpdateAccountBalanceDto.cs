namespace Internship.Shared.DTOs.Account
{
    public class UpdateAccountBalanceDto
    {
        public string DebtorIBAN { get; set; }
        public string CreditorIBAN { get; set; }
        public decimal Amount { get; set; }
        public string Reference { get; set; }
    }
}
