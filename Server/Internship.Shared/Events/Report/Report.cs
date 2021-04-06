using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Internship.Shared.Events.Report
{
    public class Report
    {
        public string IBAN { get; set; }
        public decimal OpeningBalance { get; set; }
        public decimal ClosingBalance { get; set; }
        public List<BalanceMutation> BalanceMutations { get; set; }
    }
}
