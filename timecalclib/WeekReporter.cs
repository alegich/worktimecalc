using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace timecalclib
{
   public class WeekReporter: WeekReportable
   {
      public WeekReporter(List<Reportable> records)
      {
         this.records = records;
      }

      public TimeSpan TodaysBalance()
      {
         TimeSpan retVal = new TimeSpan();
         int count = 0;
         foreach (Reportable item in records)
         {
            if (item.WorkStarted().Date != DateTime.Now.Date)
            {
               retVal += item.WholeDayTime();
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
            retVal += item.WholeDayTime();
         }

         return new TimeSpan(records.Count * 8, 0, 0) - retVal;
      }

      private readonly List<Reportable> records;
   }
}
