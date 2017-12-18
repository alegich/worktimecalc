using System;

namespace timecalclib
{
   public interface WeekReportable
   {
      TimeSpan TodaysBalance();
      TimeSpan TimeLeft();
   }
}
