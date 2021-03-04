using System;

namespace Internship.FileService.Domain.Models
{
    /// <summary>
    /// Transaction File model
    /// </summary>
    public class Transaction
    {
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Creditor { get; set; }
        public string Debtor { get; set; }
        public Guid TransactionId { get; set; }
    }
}