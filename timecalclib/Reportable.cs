using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace timecalclib
{
   public interface Reportable
   {
      TimeSpan WholeDayTime();
      TimeSpan WorkDuration();
      TimeSpan AwayDuration();
      TimeSpan LunchDuration();
      long WorkMillis();
      long AwayMillis();
      DateTime WorkStarted();
      DateTime WorkEnded();
      DateTime LunchStarted();
      DateTime LunchEnded();
      List<KeyValuePair<DateTime, DateTime>> AwaySessions();
   }
}
