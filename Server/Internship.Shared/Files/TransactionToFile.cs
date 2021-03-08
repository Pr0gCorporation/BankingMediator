using System;
using System.Xml.Serialization;

namespace Internship.Shared.Files
{
    [XmlRoot(ElementName = "Transaction")]
    public class TransactionToFile
    {
        public AccountToFile Debtor { get; set; }
        public AccountToFile Creditor { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public Guid TransactionId { get; set; }
    }
}