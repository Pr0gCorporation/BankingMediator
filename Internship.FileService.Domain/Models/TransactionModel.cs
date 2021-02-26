using System;
using System.IO.Enumeration;

namespace Internship.FileService.Domain.Models
{
    public class TransactionModel
    {
        public int FileId { get; set; }
        public DateTime Date { get; set; }
        public string Type { get; set; }
        public string FileName { get; set; }
        public byte[] File { get; set; }
    }
}