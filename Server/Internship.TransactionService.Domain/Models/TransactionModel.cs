using System;

namespace Internship.TransactionService.Domain.Models
{
    public class TransactionModel
    {
        public int Id { get; set; }
        public AccountModel Debtor { get; set; }
        public AccountModel Creditor { get; set; }
        public decimal Amount { get; set; }
        public Guid TransactionId { get; set; }
    }
}