﻿using System;
using System.Collections.Generic;

namespace Internship.Shared.Events.Report
{
    public class EndOfDayReportedEvent
    {
        public DateTime Date { get; set; }
        public IEnumerable<AccountReport> AccountReports { get; set; }
    }
}
