using System;

namespace Internship.FileService.Domain.Models
{
    public class TransactionModel
    {
        public Guid TransactionId { get; set; }
        public DateTime Date { get; set; }
        public string Creditor { get; set; }
        public string Debtor { get; set; }
        public string FileName { get; set; }
        public byte[] File { get; set; }
    }
}