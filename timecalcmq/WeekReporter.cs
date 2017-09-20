using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace timecalcmq
{
   public class WeekReporter : timecalclib.WeekReportable
   {
      public TimeSpan TimeLeft()
      {
         throw new NotImplementedException();
      }

      public TimeSpan TodaysBalance()
      {
         throw new NotImplementedException();
      }
   }
}
