using System;

namespace Internship.FileService.Domain.Models
{
    /// <summary>
    /// Transaction File model
    /// </summary>
    public class Transaction
    {
        public int TransactionId { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Creditor { get; set; }
        public string Debtor { get; set; }
    }
}