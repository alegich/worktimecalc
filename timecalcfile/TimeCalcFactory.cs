using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using timecalclib;

namespace timecalcfile
{
    public class TimeCalcFactory : timecalclib.TimeCalcFactory
    {
        private readonly string path = @"D:\data\docs\time";

        public Reportable CreateReporter(DateTime day)
        {
            return new FileBasedReporter(path, day);
        }

        public WeekReportable CreateWeekReporter(DateTime date)
        {
            return new FileBasedWeekReporter(date, path);
        }

        public Writable CreateWriter()
        {
            return new FileWriter(path);
        }
    }
}
