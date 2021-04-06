using System;

namespace Internship.FileService.Infrastructure.FileModels
{
    public class TransactionToFile
    {
        public AccountToFile Debtor { get; set; }
        public AccountToFile Creditor { get; set; }
        public decimal Amount { get; set; }
        public Guid EndToEndId { get; set; }
    }
}