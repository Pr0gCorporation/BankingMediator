using System;

namespace Internship.TransactionService.Domain.Models
{
    public class TransactionModel
    {
        public int DebtorId { get; set; }
        public int CreditorId { get; set; }
        public decimal Amount { get; set; } = 0;
        public Guid TransactionId { get; set; }
        public int Id { get; set; }
    }
}