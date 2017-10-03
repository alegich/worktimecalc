using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apache.NMS;

namespace timecalcmq
{
   class QueueReader
   {
      private IConnection connection;

      private readonly string queueName;

      public QueueReader(IConnection connection, string queueName)
      {
         this.connection = connection;
         this.queueName = queueName;
      }
   }
}
