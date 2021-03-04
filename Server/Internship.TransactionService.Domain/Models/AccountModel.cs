namespace Internship.TransactionService.Domain.Models
{
    public class AccountModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AccountNumber { get; set; }
        public string BankId { get; set; } = "0000";
    }
}