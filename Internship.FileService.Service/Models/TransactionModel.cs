using System;
using System.Xml.Serialization;

namespace Internship.FileService.Service.Models
{
    [XmlRoot(ElementName = "Transaction", Namespace = "")]
    public class TransactionModel
    {
        [XmlElement("Id")]
        public Guid Id { get; set; }
        [XmlElement("Date")]
        public DateTime Date { get; set; }
        [XmlElement("From")]
        public string From { get; set; }
        [XmlElement("To")]
        public string To { get; set; }
        public string FileName { get; set; }
    }
}