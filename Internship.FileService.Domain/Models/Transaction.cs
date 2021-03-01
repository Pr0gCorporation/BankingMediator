using System;

namespace Internship.FileService.Domain.Models
{
    public class Transaction
    {
        public int TransactionId { get; set; }
        public decimal Sum { get; set; }
        public DateTime Date { get; set; }
        public string Creditor { get; set; }
        public string Debtor { get; set; }
    }
}