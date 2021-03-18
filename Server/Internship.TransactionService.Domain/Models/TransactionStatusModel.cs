using System;

namespace Internship.TransactionService.Domain.Models
{
    public class TransactionStatusModel
    {
        public int Id { get; set; }
        public int TransactionId { get; set; }
        public string Status { get; set; }
        public string Reason { get; set; }
        public DateTime DateStatusChanged { get; set; }
    }
}