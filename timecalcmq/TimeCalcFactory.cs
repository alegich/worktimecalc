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
      private readonly QueueCommunicator queueCommunicator;

      public TimeCalcFactory()
      {
         queueCommunicator = new QueueCommunicator();
      }

      public Reportable CreateReporter(DateTime day)
      {
         return new MessageBasedReporter(queueCommunicator.GetQueueClient(), day);
      }

      public WeekReportable CreateWeekReporter(DateTime date)
      {
         throw new NotImplementedException();
      }

      public Writable CreateWriter()
      {
         return new Writer(queueCommunicator.GetQueueClient());
      }
   }
}
