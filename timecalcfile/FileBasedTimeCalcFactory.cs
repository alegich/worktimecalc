using System;
using timecalclib;

namespace timecalcfile
{
    public class FileBasedTimeCalcFactory : TimeCalcFactory
    {
        private readonly string path = @"D:\data\docs\time";

        public Reportable CreateReporter(DateTime day)
        {
            return new FileBasedReporter(path, day);
        }

        public WeekReportable CreateWeekReporter(DateTime date)
        {
            return new FileBasedWeekReporter(path, date);
        }

        public Writable CreateWriter()
        {
            return new FileWriter(path);
        }
    }
}
