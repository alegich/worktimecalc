using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace timecalclib
{
   public class FileBasedWeekReporter : WeekReportable
   {
      private WeekReporter week;
      public FileBasedWeekReporter(DateTime date, string folder)
      {
         List<Reportable> records = new List<Reportable>();
         for (int i = 0; i < SpentDaysCount(date); ++i)
         {
            records.Add(new FileBasedReporter(folder, StartOfWeek(date).AddDays(i)));
         }

         week = new WeekReporter(records);
      }

   public TimeSpan TimeLeft()
      {
         return week.TimeLeft();
      }

      public TimeSpan WholeWeekTime()
      {
         throw new NotImplementedException();
      }

      protected DateTime StartOfWeek(DateTime dt)
      {
         int diff = dt.DayOfWeek - DayOfWeek.Monday;
         if (diff < 0)
         {
            diff += 7;
         }
         return dt.AddDays(-1 * diff).Date;
      }

      protected int SpentDaysCount(DateTime dt)
      {
         return dt.DayOfWeek - DayOfWeek.Monday + 1;
      }
   }
}
