using System;

namespace Internship.TransactionService.Domain.Models
{
    public class TransactionStatusModel
    {
        public int Id { get; set; }
        public Guid TransactionId { get; set; }
        public string Status { get; set; }
        public string Reason { get; set; }
        public DateTime Date { get; set; }
    }
}