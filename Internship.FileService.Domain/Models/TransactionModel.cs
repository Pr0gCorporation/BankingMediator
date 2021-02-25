using System;

namespace Internship.FileService.Domain.Models
{
    public class TransactionModel
    {
        public Guid TransactionId { get; set; }
        public DateTime Date { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string FileName { get; set; }
        public byte[] File { get; set; }
    }
}