using System;

namespace Internship.Shared.DTOs.Transaction
{
    public class IncomingTransactionDto
    {
        public string DebtorFirstName { get; set; }
        public string DebtorLastName { get; set; }
        public string DebtorAccountNumber { get; set; }
        public string DebtorBankId { get; set; }
        public string CreditorFirstName { get; set; }
        public string CreditorLastName { get; set; }
        public string CreditorAccountNumber { get; set; }
        public string CreditorBankId { get; set; }
        public decimal Amount { get; set; }
        public Guid TransactionId { get; set; }
    }
}
