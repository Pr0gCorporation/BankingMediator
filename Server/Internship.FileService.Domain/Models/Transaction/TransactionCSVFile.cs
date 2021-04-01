using System;

namespace Internship.FileService.Domain.Models.Transaction
{
    public class TransactionCSVFile
    {
        public int FileId { get; set; }
        public DateTime Date { get; set; }
        public string DebtorFirstName { get; set; }
        public string DebtorLastName { get; set; }
        public string DebtorAccountNumber { get; set; }
        public string DebtorBankId { get; set; }
        public string CreditorFirstName { get; set; }
        public string CreditorLastName { get; set; }
        public string CreditorAccountNumber { get; set; }
        public string CreditorBankId { get; set; }
        public decimal Amount { get; set; }
        public Guid EndToEndId { get; set; }
    }
}
