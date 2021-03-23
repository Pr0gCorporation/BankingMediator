using System;
using System.Collections.Generic;
using System.Text;

namespace Internship.AccountService.Domain.Models
{
    public class CashbookRecordModel
    {
        public int Id { get; set; }
        public int CashbookId { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public string OriginReference { get; set; }
    }
}
