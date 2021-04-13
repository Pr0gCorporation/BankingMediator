using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Internship.Shared.Events.Report
{
    public class AccountReport
    {
        public string IBAN { get; set; }
        public decimal OpeningBalance { get; set; }
        public decimal ClosingBalance { get; set; }
        public IEnumerable<BalanceMutation> BalanceMutations { get; set; }
    }
}
