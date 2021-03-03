using System;

namespace Internship.TransactionService.API.DTOs.Transaction
{
    public class TransactionReadDto
    {
        public int DebtorId { get; set; }
        public int CreditorId { get; set; }
        public decimal Amount { get; set; } = 0;
        public Guid TransactionId { get; set; }
    }
}