using System;
using Internship.TransactionService.API.DTOs.Account;

namespace Internship.TransactionService.API.DTOs.Transaction
{
    public class TransactionCreateDto
    {
        public int Id { get; set; }
        public int DebtorId { get; set; }
        public int CreditorId { get; set; }
        public decimal Amount { get; set; } = 0;
        public Guid TransactionId { get; set; }
    }
}