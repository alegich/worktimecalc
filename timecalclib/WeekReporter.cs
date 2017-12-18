using System;
using System.Collections.Generic;

namespace timecalclib
{
   public class WeekReporter: WeekReportable
   {
      public TimeSpan TodaysBalance()
      {
         TimeSpan retVal = new TimeSpan();
         int count = 0;
         foreach (Reportable item in records)
         {
            if (item.WorkStarted().Date != DateTime.Now.Date)
            {
               retVal += item.WorkDuration();
               ++count;
            }
         }

         return retVal - new TimeSpan(count * 8, 0, 0);
      }

      public TimeSpan TimeLeft()
      {
         TimeSpan retVal = new TimeSpan();
         foreach (Reportable item in records)
         {
            retVal += item.WorkDuration();
         }

         return new TimeSpan(records.Count * 8, 0, 0) - retVal;
      }

      protected int SpentDaysCount(DateTime dt)
      {
         return dt.DayOfWeek - DayOfWeek.Monday + 1;
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

      protected List<Reportable> GetRecords()
      {
         return records;
      }

      private readonly List<Reportable> records = new List<Reportable>();
   }
}
