using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Internship.Shared.Events.Report
{
    public class BalanceMutation
    {
        public string DebtorIBAN { get; set; }
        public string CreditorIBAN { get; set; }
        public decimal Amount { get; set; }
        public string OriginalReference { get; set; }
    }
}
