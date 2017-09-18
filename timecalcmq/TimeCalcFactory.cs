using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using timecalclib;

namespace timecalcmq
{
   public class TimeCalcFactory : timecalclib.TimeCalcFactory
   {
      public Reportable CreateReporter(DateTime day)
      {
         throw new NotImplementedException();
      }

      public WeekReportable CreateWeekReporter(DateTime date)
      {
         throw new NotImplementedException();
      }

      public Writable CreateWriter()
      {
         throw new NotImplementedException();
      }
   }
}
