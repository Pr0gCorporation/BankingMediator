namespace Internship.AccountService.Domain.Models
{
    public class CashbookModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public decimal Amount { get; set; }
    }
}
