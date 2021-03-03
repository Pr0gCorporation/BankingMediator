using System;
using Internship.TransactionService.API.DTOs.Account;

namespace Internship.TransactionService.API.DTOs.Transaction
{
    public class TransactionCreateDto
    {
        public int Id { get; set; }
        public AccountCreateDto Debtor { get; set; }
        public AccountCreateDto Creditor { get; set; }
        public decimal Amount { get; set; } = 0;
        public Guid TransactionId { get; set; }
    }
}