using System;

namespace Internship.FileService.Service.DTOs
{
    public class TransactionDto
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public string FileName { get; set; }
        public string From { get; set; }
        public string To { get; set; }
    }
}