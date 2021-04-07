using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Internship.FileService.Infrastructure.FileModels.Report
{
    [XmlRoot(ElementName = "EndOfDayReport")]
    public class EndOfDayReportXmlFile
    {
        public DateTime Date { get; set; }
        [XmlArrayItem("Report")]
        public List<AccountReportXmlFile> Reports { get; set; }
    }
}
