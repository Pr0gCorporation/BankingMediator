using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Internship.FileService.Infrastructure.FileModels
{
    /// <summary>
    /// TransactionFileModel is used to serialize/deserialize xml and json files
    /// </summary>
    [XmlRoot(ElementName = "TransactionFile")]
    public class TransactionFileModel
    {
        public int FileId { get; set; }
        public DateTime Date { get; set; }
        [XmlArrayItem("Transaction")]
        public List<TransactionToFile> Transactions { get; set; }
    }
}
