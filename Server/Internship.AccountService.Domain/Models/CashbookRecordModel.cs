using System;

namespace Internship.AccountService.Domain.Models
{
    public class CashbookRecordModel
    {
        public int Id { get; set; }
        public int CashbookIdDebtor { get; set; }
        public int CashbookIdCreditor { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public string OriginReference { get; set; }
    }
}
