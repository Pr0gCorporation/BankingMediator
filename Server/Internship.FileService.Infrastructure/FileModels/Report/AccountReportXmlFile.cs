using System.Collections.Generic;
using System.Xml.Serialization;

namespace Internship.FileService.Infrastructure.FileModels.Report
{
    public class AccountReportXmlFile
    {
        public string IBAN { get; set; }
        public decimal OpeningBalance { get; set; }
        public decimal ClosingBalance { get; set; }
        [XmlArrayItem("BalanceMutation")]
        public List<BalanceMutationXmlFile> BalanceMutations { get; set; }
    }
}
