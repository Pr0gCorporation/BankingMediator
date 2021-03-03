namespace Internship.TransactionService.API.DTOs.Account
{
    public class AccountCreateDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AccountNumber { get; set; }
        public string BankId { get; set; }
    }
}