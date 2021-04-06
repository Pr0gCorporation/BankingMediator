using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Internship.Shared.Events.Report
{
    public class EndOfDayReportedEvent
    {
        public DateTime Date { get; set; }
        public List<Report> Reports { get; set; }
    }
}
