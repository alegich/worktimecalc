using System;
using System.Collections.Generic;
using timecalclib;

namespace timecalcfile
{
   public class FileBasedWeekReporter : WeekReporter
   {
      public FileBasedWeekReporter(string folder, DateTime date)
      {
         List<Reportable> records = new List<Reportable>();

         for (int i = 0; i < SpentDaysCount(date); ++i)
         {
            records.Add(new FileBasedReporter(folder, StartOfWeek(date).AddDays(i)));
         }

         GetRecords().AddRange(records);
      }
   }
}
