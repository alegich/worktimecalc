using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace timecalclib
{
    public interface TimeCalcFactory
    {
        Reportable CreateReporter(DateTime day);

        WeekReportable CreateWeekReporter(DateTime date);

        Writable CreateWriter();
    }
}
