using System;

namespace Internship.FileService.Domain.Models
{
    public class TransactionModel
    {
        public Guid PaymentId { get; set; }
        public decimal Sum { get; set; }
        public DateTime Date { get; set; }
        public string Creditor { get; set; }
        public string Debtor { get; set; }
    }
}