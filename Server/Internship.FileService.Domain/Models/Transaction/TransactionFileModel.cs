using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Internship.FileService.Domain.Models.Transaction
{
    [XmlRoot(ElementName = "TransactionFile")]
    public class TransactionFileModel
    {
        public int FileId { get; set; }
        public DateTime Date { get; set; }
        [XmlArrayItem("Transaction")]
        public List<TransactionToFile> Transactions { get; set; }
    }
}
