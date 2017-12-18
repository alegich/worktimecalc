using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace timecalcmq
{
   class MessageBasedReporter : timecalclib.TimeReporter
   {
      public MessageBasedReporter(QueueClient queueReader, DateTime day)
      {
         List<string> content = ((IList<string>)queueReader.GetDayLog(day)).ToList();
         GetRecords().AddRange(ConvertFromStrings(content));
      }
   }
}
