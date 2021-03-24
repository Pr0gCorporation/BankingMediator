using System;

namespace Internship.FileService.Domain.Models.Transaction
{
    public class TransactionToFile
    {
        public AccountToFile Debtor { get; set; }
        public AccountToFile Creditor { get; set; }
        public decimal Amount { get; set; }
        public Guid EndToEndId { get; set; }
    }
}