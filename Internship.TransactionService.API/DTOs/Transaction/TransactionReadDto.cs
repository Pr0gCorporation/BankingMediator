using System;
using Internship.TransactionService.API.DTOs.Account;

namespace Internship.TransactionService.API.DTOs.Transaction
{
    public class TransactionReadDto
    {
        public AccountReadDto Debtor { get; set; }
        public AccountReadDto Creditor { get; set; }
        public decimal Amount { get; set; }
        public Guid TransactionId { get; set; }
    }
}