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

      public TimeSpan WholeWeekTime()
      {
         throw new NotImplementedException();
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
